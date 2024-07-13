using Ado.net.Constants;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ado.net.Services
{
    public static class CountryService
    {
        public static void GetAllCountries()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStrings.Default))
            {
                connection.Open();

                var command = new SqlCommand("SELECT * FROM COUNTRIES", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = Convert.ToInt32(reader["Id"]);
                        string name = Convert.ToString(reader["Name"]);
                        decimal area = Convert.ToDecimal(reader["Area"]);

                        Messages.PrintMessage("Id", id.ToString());
                        Messages.PrintMessage("Name", name);
                        Messages.PrintMessage("Area", area.ToString());


                    }

                }
            }
        }
        public static void AddCountry()
        {
            Messages.InputMessage("country name");
            string name = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(name))
            {
                using (SqlConnection connection = new SqlConnection(ConnectionStrings.Default))
                {
                    connection.Open();
                    var selectCommand = new SqlCommand("SELECT * FROM WHERE Name=@name, connection");
                    selectCommand.Parameters.AddWithValue("name", name);
                    try
                    {
                        int id = Convert.ToInt32(selectCommand.ExecuteScalar());
                        if (id > 0)
                            Messages.AlreadyExistMessage("Country", name);
                        else
                        {

                            Messages.InputMessage("contry area");
                            string areaInput = Console.ReadLine();
                            decimal area;
                            bool isSucceeded = decimal.TryParse(areaInput, out area);
                            if (isSucceeded)
                            {



                                var command = new SqlCommand("INSERT INTO Countries VALUES(@name, @area)", connection);
                                command.Parameters.AddWithValue("@name", name);
                                command.Parameters.AddWithValue("@area", area);


                                var affectedRows = command.ExecuteNonQuery();
                                if (affectedRows > 0)
                                    Messages.SuccesMessage("Country", name);
                                else
                                    Messages.ErrorOccuredMessage();
                            }
                        }
                    }

                    catch (Exception)
                    {
                        Messages.ErrorOccuredMessage();
                    }
                }
            }
            else
                Messages.InvalidInputMessage("Country name");

        }

        public static void UpdateCountry()
        {
            GetAllCountries();

            Messages.InputMessage("Country name");
            string name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                using (SqlConnection connection = new SqlConnection(ConnectionStrings.Default))
                {
                    connection.Open();
                    var command = new SqlCommand("SELECT * FROM COUNTRIES WHERE Name=@name", connection);
                    command.Parameters.AddWithValue("@name", name);
                    try
                    {
                        int id = Convert.ToInt32(command.ExecuteScalar());
                        if (id > 0)
                        {
                        NameWantToChangeSection: Messages.PrintWantToChangeMessage("name");
                            bool hasUpdates;
                            var choiceForNAme = Console.ReadLine();
                            char choice;
                            bool isSucceeded = char.TryParse(choiceForNAme, out choice);
                            if (isSucceeded && choice.IsVAlidChoice())
                            {
                                string newName = string.Empty;
                                if (choice.Equals('y'))
                                {
                                InputNewNameSection: Messages.InputMessage("new name");
                                    newName = Console.ReadLine();
                                    if (!string.IsNullOrWhiteSpace(newName))
                                    {
                                        var alreadyExistsCommand = new SqlCommand("SELECT * FROM Countries WHERE Name=@name AND Id !=@id", connection);
                                        alreadyExistsCommand.Parameters.AddWithValue("@name", id);
                                        alreadyExistsCommand.Parameters.AddWithValue("@id", newName);

                                        int existId = Convert.ToInt32(alreadyExistsCommand.ExecuteScalar());
                                        if (existId > 0)
                                        {
                                            Messages.AlreadyExistMessage("Country", newName);
                                            goto NameWantToChangeSection;
                                        }

                                    }
                                    else
                                    {
                                        Messages.InvalidInputMessage("New name");
                                        goto InputNewNameSection;
                                    }
                                }
                            AreaWantToChangeSection: Messages.PrintWantToChangeMessage("area");
                                var choiceForArea = Console.ReadLine();
                                isSucceeded = char.TryParse(choiceForArea, out choice);
                                decimal newArea = default;
                                if (isSucceeded && choice.IsVAlidChoice())
                                {

                                    if (choice.Equals('y'))
                                    {
                                    InputNewArea: Messages.InputMessage("New area");
                                        var newAreaInput = Console.ReadLine();
                                        isSucceeded = decimal.TryParse(newAreaInput, out newArea);
                                        if (!isSucceeded)
                                        {
                                            Messages.InvalidInputMessage("New area");
                                            goto InputNewArea;
                                        }
                                    }

                                }
                                else
                                {
                                    Messages.InvalidInputMessage("New area");
                                    goto AreaWantToChangeSection;
                                };

                                var updateComand = new SqlCommand("Update Countries SET ", connection);
                                if (newName != string.Empty || newArea != default)
                                {
                                    bool isRequiredcomma = false;
                                    if (newName != string.Empty)
                                    {
                                        updateComand.CommandText = updateComand.CommandText + "Name = @name";
                                        updateComand.Parameters.AddWithValue("@name", newName);
                                    }

                                    if (newArea != default)
                                    {
                                        string commaText = isRequiredcomma ? "," : "";
                                        updateComand.CommandText = updateComand.CommandText + $"{commaText}Area=@area";
                                        updateComand.Parameters.AddWithValue("@area", newArea);
                                    }
                                    updateComand.CommandText = updateComand.CommandText + " WHERE id=@id";
                                    updateComand.Parameters.AddWithValue("@id", id);
                                    int affectedRows = Convert.ToInt32(updateComand.ExecuteNonQuery());
                                    if (affectedRows > 0)
                                    {
                                        Messages.SuccesMessage("Country", newName);
                                    }
                                    else
                                        Messages.ErrorOccuredMessage();
                                }
                            }
                            else
                                Messages.InvalidInputMessage("Choice");

                        }
                        else
                            Messages.NotFoundMessage("Country", name);
                    }
                    catch
                    {
                        Messages.ErrorOccuredMessage();
                    }
                }
            }
            else
                Messages.InvalidInputMessage("Country name");
        }

        public static void DeleteCountry()
        {
            GetAllCountries();

            string name = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(name))
            {
                using (SqlConnection connection = new SqlConnection(ConnectionStrings.Default))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SELECT * FROM WHERE Name=@name", connection);
                    command.Parameters.AddWithValue("@name", name);

                    try
                    {
                        int id = Convert.ToInt32(command.ExecuteScalar());
                        if (id > 0)
                        {
                            SqlCommand deleteCommand = new SqlCommand("DELETE Countries WHERE Id=@id", connection);
                            deleteCommand.Parameters.AddWithValue("@id", id);

                            int affectedRows = deleteCommand.ExecuteNonQuery();
                            if (affectedRows > 0)
                            {
                                Messages.SuccesDeleteMessage("Country", name);
                            }
                            else
                            {
                                Messages.ErrorOccuredMessage();
                            }
                        }
                        else
                            Messages.NotFoundMessage("Country", name);
                    }
                    catch (Exception)
                    {
                        Messages.ErrorOccuredMessage();
                    }
                }

            }
            else
                Messages.InvalidInputMessage("Country Name");
        }

        public static void DetailsofCountry()
        {
            GetAllCountries();

            string name = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(name))
            {
                using (SqlConnection connection = new SqlConnection(ConnectionStrings.Default))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SELECT * FROM Countries WHERE Name=@name", connection);
                    command.Parameters.AddWithValue("@name", name);
                    try
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();

                                Messages.PrintMessage("Id", Convert.ToString(reader["Id"]));
                                Messages.PrintMessage("Name", Convert.ToString(reader["Name"]));
                                Messages.PrintMessage("Area", Convert.ToString(reader["Area"]));
                            }
                        }
                    }
                    catch (Exception)
                    {

                        Messages.ErrorOccuredMessage();
                    }

                }
            }
            else
                Messages.InvalidInputMessage("name");
        }
    }
}
