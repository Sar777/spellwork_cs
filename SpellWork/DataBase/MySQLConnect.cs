﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using SpellWork.Extensions;
using SpellWork.Properties;

namespace SpellWork.Database
{
    public static class MySqlConnection
    {
        private static MySql.Data.MySqlClient.MySqlConnection _conn;
        private static MySqlCommand _command;

        public static bool Connected { get; private set; }
        public static List<string> Dropped = new List<string>();
        public static List<SpellProcEventEntry> SpellProcEvent = new List<SpellProcEventEntry>();

        private static String ConnectionString
        {
            get
            {
                if (Settings.Default.Host == ".")
                    return String.Format("Server=localhost;Pipe={0};UserID={1};Password={2};Database={3};CharacterSet=utf8;ConnectionTimeout=5;ConnectionProtocol=Pipe;",
                        Settings.Default.PortOrPipe, Settings.Default.User, Settings.Default.Pass, Settings.Default.WorldDbName);

                return String.Format("Server={0};Port={1};UserID={2};Password={3};Database={4};CharacterSet=utf8;ConnectionTimeout=5;",
                    Settings.Default.Host, Settings.Default.PortOrPipe, Settings.Default.User, Settings.Default.Pass, Settings.Default.WorldDbName);
            }
        }

        private static String GetSpellName(uint id)
        {
            if (DBC.DBC.SpellInfoStore.ContainsKey(id))
                return DBC.DBC.SpellInfoStore[id].SpellNameRank;

            Dropped.Add(String.Format("DELETE FROM `spell_proc_event` WHERE `entry` IN ({0});\r\n", id.ToUInt32()));
            return String.Empty;
        }

        public static void SelectProc(string query)
        {
            using (_conn = new MySql.Data.MySqlClient.MySqlConnection(ConnectionString))
            {
                _command = new MySqlCommand(query, _conn);
                _conn.Open();
                SpellProcEvent.Clear();

                using (var reader = _command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        uint spellId = reader.GetUInt32(0);
                        SpellProcEvent.Add(new SpellProcEventEntry
                        {
                            Id                  = spellId,
                            SpellName           = GetSpellName(spellId),
                            SchoolMask          = reader.GetByte(1),
                            SpellFamilyName     = reader.GetUInt16(2),
                            SpellFamilyMask     = new[]
                            {
                                reader.GetUInt32(3),
                                reader.GetUInt32(4),
                                reader.GetUInt32(5)
                            },
                            ProcFlags           = reader.GetUInt32(6),
                            ProcEx              = reader.GetUInt32(7),
                            PpmRate             = reader.GetFloat(8),
                            CustomChance        = reader.GetFloat(9),
                            Cooldown            = reader.GetUInt32(10)
                        });
                    }
                }
            }
        }

        public static void Insert(string query)
        {
            _conn    = new MySql.Data.MySqlClient.MySqlConnection(ConnectionString);
            _command = new MySqlCommand(query, _conn);
            _conn.Open();
            _command.ExecuteNonQuery();
            _command.Connection.Close();
        }

        public static void TestConnect()
        {
            if (!Settings.Default.UseDbConnect)
            {
                Connected = false;
                return;
            }

            try
            {
                _conn = new MySql.Data.MySqlClient.MySqlConnection(ConnectionString);
                _conn.Open();
                _conn.Close();
                Connected = true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(string.Format("Errno {0}{1}{2}", ex.Number, Environment.NewLine, ex.Message));
                Connected = false;
            }
            catch
            {
                Connected = false;
            }
        }
    }
}
