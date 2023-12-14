using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Diagnostics;

namespace KormosAron_Etlap
{
    public class EtlapService
    {
        MySqlConnection connection;

        public EtlapService()
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = "localhost";
            builder.Database = "etlapdb";
            builder.UserID = "root";
            builder.Password = "mypassword";
            connection = new MySqlConnection(builder.ToString());
        }

        public List<Etel> GetAll()
        {
            List<Etel> etelek = new List<Etel>();
            OpenConn();
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM etlap", connection);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Etel etel = new Etel();
                etel.Id = reader.GetInt32("id");
                etel.Nev = reader.GetString("nev");
                etel.Leiras = reader.GetString("leiras");
                etel.Ar = reader.GetInt32("ar");
                etel.Kategoria = reader.GetString("kategoria");
                etelek.Add(etel);
            }
            CloseConn();
            return etelek;
        }

        public bool OpenConn()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        public bool CloseConn()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        public bool Insert(Etel etel)
        {
            OpenConn();
            MySqlCommand cmd = new MySqlCommand("INSERT INTO etlap (nev, leiras, ar, kategoria) VALUES (@nev, @leiras, @ar, @kategoria)", connection);
            cmd.Parameters.AddWithValue("@nev", etel.Nev);
            cmd.Parameters.AddWithValue("@leiras", etel.Leiras);
            cmd.Parameters.AddWithValue("@ar", etel.Ar);
            cmd.Parameters.AddWithValue("@kategoria", etel.Kategoria);
            int result = cmd.ExecuteNonQuery();
            CloseConn();
            return result > 0;
        }

        public bool Delete(Etel etel)
        {
            OpenConn();
            MySqlCommand cmd = new MySqlCommand("DELETE FROM etlap WHERE id = @id", connection);
            cmd.Parameters.AddWithValue("@id", etel.Id);
            int result = cmd.ExecuteNonQuery();
            CloseConn();
            return result > 0;
        }

        public bool Modify(Etel etel, int aremeles, bool szazalekos)
        {
            OpenConn();
            string builder;
            if (szazalekos)
            {
                etel.Ar = (int)(etel.Ar * (1 + aremeles / 100.0));
            }
            else
            {
                etel.Ar += aremeles;
            }
            builder = "UPDATE etlap SET ar = @ar WHERE id = @id";
            MySqlCommand cmd = new MySqlCommand(builder, connection);

            cmd.Parameters.AddWithValue("@ar", etel.Ar);
            cmd.Parameters.AddWithValue("@id", etel.Id);
            Debug.WriteLine(etel.Ar);
            int result = cmd.ExecuteNonQuery();
            CloseConn();
            return result > 0;

        }
    }
}
