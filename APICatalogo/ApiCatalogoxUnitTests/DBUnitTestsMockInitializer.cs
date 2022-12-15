using APICatalogo.Context;
using APICatalogo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiCatalogoxUnitTests
{
    public class DBUnitTestsMockInitializer
    {
        public DBUnitTestsMockInitializer()
        {}

        public void Seed(AppDbContext context)
        {
            context.Categorias.Add(new Categoria { 
                CategoriaId = 999,
                Nome = "Bebidas",
                ImagemUrl = "bebidas.jpg."
            });

            context.Categorias.Add(new Categoria
            {
                CategoriaId = 998,
                Nome = "Sucos",
                ImagemUrl = "sucos.jpg."
            });

            context.Categorias.Add(new Categoria
            {
                CategoriaId = 997,
                Nome = "Doces",
                ImagemUrl = "doces.jpg."
            });

            context.Categorias.Add(new Categoria
            {
                CategoriaId = 996,
                Nome = "Salgados",
                ImagemUrl = "salgados.jpg."
            });

            context.Categorias.Add(new Categoria
            {
                CategoriaId = 995,
                Nome = "Tortas",
                ImagemUrl = "tortas.jpg."
            });

            context.Categorias.Add(new Categoria
            {
                CategoriaId = 995,
                Nome = "Bolos",
                ImagemUrl = "bolos.jpg."
            });
        }
    }
}
