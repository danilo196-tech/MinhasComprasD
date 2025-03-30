using MinhasComprasD.Models;
using SQLite;

namespace MinhasComprasD.Helpers
{
    public class SQLiteDatabaseHelper
    //Essa classe tem como objetivo gerenciar operações de banco de dados SQLite de maneira assíncrona.
    {
        readonly SQLiteAsyncConnection _connection;
        //_connection: A variável de instância que representa a conexão assíncrona com o banco de dados SQLite.
        public SQLiteDatabaseHelper(string path)
        //Construtor da classe que recebe o caminho do banco de dados (path) e cria uma conexão assíncrona (_connection) com o SQLite.
        {
            _connection = new SQLiteAsyncConnection(path);
            _connection.CreateTableAsync<Produto>().Wait();
            /*  Cria a tabela Produto no banco de dados, se ela ainda não existir. 
               O método Wait() é usado para garantir que a tabela seja criada antes 
             de qualquer outra operação ser executada (bloqueando a execução até a operação ser concluída).
            */

        }

        public Task<int> Insert(Produto p)
        //Insere um objeto Produto no banco de dados.
        {
            return _connection.InsertAsync(p);
            //Retorna uma Task<int>, que pode ser usada para saber o número de linhas afetadas pela operação
        }
        public Task<List<Produto>> Update(Produto p)
        // Atualiza as informações de um produto no banco de dados.
        {
            string sql = "UPDATE Produto SET Descricao=?, Quantidade=?, Preco=? WHERE Id=?";
            //A string SQL realiza a atualização dos campos Descricao, Quantidade, e Preco na tabela Produto, com base no Id do produto.
            return _connection.QueryAsync<Produto>(
                sql, p.Descricao, p.Quantidade, p.Preco, p.Id
                /*Este método executa a consulta SQL de forma assíncrona, substituindo os parâmetros ? pelos valores do objeto Produto passado
                (descricao, quantidade, preço e id).*/
                );
        }
        public Task<int> Delete(int id)
        //Exclui um produto com o Id correspondente no banco de dados.


        {
            return _connection.Table<Produto>().DeleteAsync(i => i.Id == id);
            //O método DeleteAsync é usado para deletar um registro da tabela Produto.
            //Ele aceita uma condição (expressão lambda) para determinar qual produto será excluído, no caso, o produto com o Id correspondente.
        }
        public Task<List<Produto>> GetAll()
        //Recupera todos os produtos da tabela Produto no banco de dados.
        {
            return _connection.Table<Produto>().ToListAsync();
            //O método ToListAsync() retorna todos os registros da tabela Produto como uma lista assíncrona de objetos Produto.
            //O método retorna uma Task<List<Produto>>, permitindo que a operação seja feita sem bloquear a interface do usuário ou a execução de outras tarefas.
        }
        public Task<List<Produto>> Search(string q)
        // Pesquisa produtos com base na descrição que contenha a string fornecida como parâmetro (q).
        {
            string sql = "SELECT * FROM Produto WHERE Descricao LIKE '%" + q + "%'";
            //A string SQL realiza uma busca na tabela Produto, procurando todos os produtos cujo campo Descricao contenha o texto fornecido em q.
            //O operador LIKE '%...%' é utilizado para buscar ocorrências parciais.


            return _connection.QueryAsync<Produto>(sql);
            //Este método executa a consulta SQL de forma assíncrona e retorna uma lista de objetos Produto que atendem ao critério de busca. 
        }

    }
}
