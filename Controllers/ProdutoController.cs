using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using System.Data;
using web_api_restaurante.Entidades;

namespace web_api_restaurante.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProdutoController : ControllerBase
    {
        private readonly string? _connectionString;
        //ctor atalho para criar o construtor
        public ProdutoController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private IDbConnection OpenConnection()
        {
           IDbConnection dbConnection = new SqliteConnection(_connectionString);
            dbConnection.Open();
            return dbConnection;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) 
        {
            using IDbConnection dbConnection = OpenConnection();
            string sql = "select id, nome ,descricao , imagemUrl from Produto where id = @id;";
            var result = await dbConnection.QueryAsync<Produto>(sql , new {id});

            if(result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpPost]

        public async Task<IActionResult> Post([FromBody] Produto produto)
        {
            using IDbConnection dbConnection = OpenConnection();
            string query = @"INSERT into produto(nome, descricao, imagemUrl)
                VALUES(@Nome , @Descricao, @ImagemUrl);";

            await dbConnection.ExecuteAsync(query, produto);

            return Ok();
        }
    }
}
