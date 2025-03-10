using MinhasComprasD.Models;
using SQLite;

namespace MinhasComprasD.Helpers
{
    public class SQLiteDatabaseHelper
    {
        readonly SQLiteAsyncConnection _connection;

        public SQLiteDatabaseHelper(string path) 
        {
        _connection = new SQLiteAsyncConnection(path);
        _connection.CreateTableAsync<Produto>().Wait(); 

        }

        public Task<int> Insert(Produto p)
        {
         return _connection.InsertAsync(p);  
        }
        public Task<List<Produto>> Update(Produto p)
        {
            string sql = "UPDATE Produto SET Descricao=?, Quantidade=?, Preco=? WHERE Id=?";
            return _connection.QueryAsync<Produto>(
                sql, p.Descricao, p.Quantidade, p.Preco, p.Id
                );
        }
        public Task<int> Delete(int id) 
        {
        return _connection.Table<Produto>().DeleteAsync(i => i.Id == id);
        }
        public Task<List<Produto>> GetAll() 
        {
         return _connection.Table<Produto>().ToListAsync();
        }
        public Task<List<Produto>> Search(string q)
        {
            string sql = "SELECT * Produto WHERE Descricao LIKE '%" + q + "%'";
            return _connection.QueryAsync<Produto>(sql);
                  }

    }
}
