﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;



namespace ProjetoPastelariaDoZe_2022.DAO
{

    public class Produto
    {
        public int IdProduto { get; set; }
        public string Nome { get; set; }
        public double ValorUnitario { get; set; }
        public string Descricao { get; set; }
        public byte[] Foto { get; set; }
        public Produto(int id = 0, string nome = "", double valorUnitario = 0, string descricao = "", byte[] foto = null)
        {
            IdProduto = id;
            Nome = nome;
            ValorUnitario = valorUnitario;
            Descricao = descricao;
            Foto = foto;
        }
    }
    public class ProdutoDAO
    {
        /// <summary>
        /// Utilização de mais do que um provider com o mesmo script (MySQL, SQLServer, Firebird...)
        /// </summary>
        private readonly DbProviderFactory factory;
        private string Provider { get; set; }
        private string StringConexao { get; set; }
        public ProdutoDAO(string provider, string stringConexao)
        {
            Provider = provider;
            StringConexao = stringConexao;
            factory = DbProviderFactories.GetFactory(Provider);
        }
        public void InserirDbProvider(Produto produto)
        {
            using var conexao = factory.CreateConnection(); //Cria conexão
            conexao!.ConnectionString = StringConexao; //Atribui a string de conexão
            using var comando = factory.CreateCommand(); //Cria comando
            comando!.Connection = conexao; //Atribui conexão
                                           //Adiciona parâmetros (@campo e valor)
            var nome = comando.CreateParameter();
            nome.ParameterName = "@nome";
            nome.Value = produto.Nome;
            comando.Parameters.Add(nome);
            var descricao = comando.CreateParameter();
            descricao.ParameterName = "@descricao";
            descricao.Value = produto.Descricao;
            comando.Parameters.Add(descricao);
            var valorUnitario = comando.CreateParameter();
            valorUnitario.ParameterName = "@valorUnitario";
            valorUnitario.Value = produto.ValorUnitario;
            comando.Parameters.Add(valorUnitario);
            var foto = comando.CreateParameter();
            foto.ParameterName = "@foto";
            foto.Value = produto.Foto;
            comando.Parameters.Add(foto);
            conexao.Open();
            comando.CommandText = @"INSERT INTO tb_produto(nome,descricao,valor_unitario,foto) VALUES (@nome,@descricao,@valorUnitario,@foto)";
            //Executa o script na conexão e retorna o número de linhas afetadas.
            var linhas = comando.ExecuteNonQuery();
        }
        public DataTable SelectDbProvider(Produto produto)
        {
            using var conexao = factory.CreateConnection(); //Cria conexão
            conexao!.ConnectionString = StringConexao; //Atribui a string de conexão
            using var comando = factory.CreateCommand(); //Cria comando
            comando!.Connection = conexao; //Atribui conexão
                                           //verifica se tem filtro
                                           //Adiciona parâmetro (@campo e valor)

            string auxSqlFiltro = " ";
            if (produto.IdProduto > 0)
            {
                auxSqlFiltro = " WHERE id_produto = " + produto.IdProduto + " ";
            }

            conexao.Open();
            comando.CommandText = @"" +
            "SELECT id_produto AS ID, nome AS Nome, valor_unitario AS Valor, descricao AS Descrição, foto AS Imagem " +
            "FROM tb_produto " +
            auxSqlFiltro +
            " ORDER BY nome;";

            //Executa o script na conexão e retorna as linhas afetadas.
            var sdr = comando.ExecuteReader();
            DataTable linhas = new();
            linhas.Load(sdr);
            return linhas;
        }

        public void EditarDbProvider(Produto produto)
        {
            using var conexao = factory.CreateConnection(); //Cria conexão
            conexao!.ConnectionString = StringConexao; //Atribui a string de conexão
            using var comando = factory.CreateCommand(); //Cria comando
            comando!.Connection = conexao; //Atribui conexão
                                           //Adiciona parâmetros (@campo e valor)
            var idProduto = comando.CreateParameter(); idProduto.ParameterName = "@idProduto";
            idProduto.Value = produto.IdProduto; comando.Parameters.Add(idProduto);
            var nome = comando.CreateParameter(); nome.ParameterName = "@nome";
            nome.Value = produto.Nome; comando.Parameters.Add(nome);
            var descricao = comando.CreateParameter(); descricao.ParameterName = "@descricao";
            descricao.Value = produto.Descricao; comando.Parameters.Add(descricao);
            var valorUnitario = comando.CreateParameter(); valorUnitario.ParameterName = "@valorUnitario";
            valorUnitario.Value = produto.ValorUnitario; comando.Parameters.Add(valorUnitario);
            var foto = comando.CreateParameter(); foto.ParameterName = "@foto";
            foto.Value = produto.Foto; comando.Parameters.Add(foto);
            conexao.Open();
            comando.CommandText = @"UPDATE tb_produto " +
            "SET nome = @nome, valor_unitario = @valorUnitario, descricao = @descricao, foto = @foto " +
            "WHERE id_produto = @idProduto;";
            //Executa o script na conexão e retorna as linhas afetadas.
            var linhas = comando.ExecuteNonQuery();

        }
        public void ExcluirDbProvider(Produto produto)
        {
            using var conexao = factory.CreateConnection(); //Cria conexão
            conexao!.ConnectionString = StringConexao; //Atribui a string de conexão
            using var comando = factory.CreateCommand(); //Cria comando
            comando!.Connection = conexao; //Atribui conexão
                                           //Adiciona parâmetros (@campo e valor)
            var idProduto = comando.CreateParameter();
            idProduto.ParameterName = "@idProduto";
            idProduto.Value = produto.IdProduto;
            comando.Parameters.Add(idProduto);
            conexao.Open();
            comando.CommandText = @"" +
            "DELETE FROM tb_produto " +
            "WHERE id_produto = @idProduto;";
            //Executa o script na conexão e retorna as linhas afetadas.
            var linhas = comando.ExecuteNonQuery();
        }
    }
}
