using System.Data;
using System.Data.Odbc;
using Devart.Data.Universal;
using FluentAssertions;
using Xunit;

namespace devart_odbc_filepath_issues
{
    public class OdbcConnectionStringBuilderTest
    {
        internal const string License = @"k0CkEFCuiCE5kzLrfcFyAfNrSfqJH8ovgwVLVzNejSELViDEcISCLSmAbaGzoYH90IuHpwUg8NWDV9olEQDWTKqkjJgCOvenpXDW9IMwVpr4nqtr5lAP3aDeGxWcM9i1JMsVx3KbYVSI/m3FULFnIz4TDP80RlBpCZkXd3tg25WqUtGJDvDqwtn+mJmgwH+gqr2F63wzPzE2P+eDYezo59Bp7QyWFaSqNIdDi8/scew1DT/YrrwZH2hIOP11CA09fm0VP+dRKAbsqI7KJrNHjPdJcsxCp/XglFpsw53FkkHCG4RehToO5+JPYwX1eoaD";
        public OdbcConnectionStringBuilderTest()
        {
            try
            {
                // Init License for DevArt.
                new UniConnection($"License Key={License};Provider=ODBC;Driver={{Microsoft Access Driver (*.mdb, *.accdb)}}").Open();
            }
            catch (OdbcException)
            {
                // Catch simple and continue knowing that the license key has been registered.
            }
        }

        [Theory]
        [InlineData(".\\LasVegas.mdb")] // Success
        [InlineData(".\\DataFolderWithoutSpaces\\LasVegas.mdb")] // Success
        [InlineData(".\\DataFolderWith Spaces\\LasVegas.mdb")] // Success
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
        [InlineData(".\\LasVegas.mdb")] // Success
        [InlineData(".\\DataFolderWithoutSpaces\\LasVegas.mdb")] // Success
        [InlineData(".\\DataFolderWith Spaces\\LasVegas.mdb")] // Fail
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
        [InlineData(".\\LasVegas.mdb")] // Success
        [InlineData(".\\DataFolderWithoutSpaces\\LasVegas.mdb")] // Success
        [InlineData(".\\DataFolderWith Spaces\\LasVegas.mdb")] // Fail
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