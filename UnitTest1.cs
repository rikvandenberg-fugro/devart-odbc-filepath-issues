using System.Data;
using System.Data.Odbc;
using Devart.Data.Universal;
using FluentAssertions;

namespace devart_odbc_filepath_issues
{
    public class OdbcConnectionStringBuilderTest
    {
        internal const string License = "your license here";
        public OdbcConnectionStringBuilderTest()
        {
            try
            {
                // Init License for DevArt.
                new UniConnection($"License Key={License};Provider=ODBC;Driver={{Microsoft Access Driver (*.mdb, *.accdb)}}").Open();
            }
            catch (OdbcException)
            {
                // Catch e
            }
        }

        [Theory]
        [InlineData(".\\LasVegas.mdb")]
        [InlineData(".\\DataFolderWithoutSpaces\\LasVegas.mdb")]
        [InlineData(".\\DataFolderWith Spaces\\LasVegas.mdb")]
        public void When_odbc_connection_open_should_work(string dbq)
        {
            var builder = new OdbcConnectionStringBuilder();
            builder.Driver = "{Microsoft Access Driver (*.mdb, *.accdb)}";
            builder["Dbq"] = dbq;
            using (var connection = new OdbcConnection(builder.ConnectionString))
            {
                connection.Open();
                connection.State.Should().Be(ConnectionState.Open);
            }
        }

        [Theory]
        [InlineData(".\\LasVegas.mdb")]
        [InlineData(".\\DataFolderWithoutSpaces\\LasVegas.mdb")]
        [InlineData(".\\DataFolderWith Spaces\\LasVegas.mdb")]
        public void When_uni_connection_odbc_provider_open_should_work(string dbq)
        {
            var builder = new OdbcConnectionStringBuilder();
            builder.Driver = "{Microsoft Access Driver (*.mdb, *.accdb)}";
            builder["Dbq"] = dbq;
            using (var connection = new UniConnection("Provider=ODBC;" + builder.ConnectionString))
            {
                connection.Open();
                connection.State.Should().Be(ConnectionState.Open);
            }
        }

        [Theory]
        [InlineData(".\\LasVegas.mdb")]
        [InlineData(".\\DataFolderWithoutSpaces\\LasVegas.mdb")]
        [InlineData(".\\DataFolderWith Spaces\\LasVegas.mdb")]
        public void When_uni_connection_with_builder_open_should_work(string dbq)
        {
            var builder = new UniConnectionStringBuilder();
            builder.Provider = "ODBC";
            builder["Driver"] = "{Microsoft Access Driver (*.mdb, *.accdb)}";
            builder["Dbq"] = dbq;
            using (var connection = new UniConnection(builder.ConnectionString))
            {
                connection.Open();
                connection.State.Should().Be(ConnectionState.Open);
            }
        }
    }
}