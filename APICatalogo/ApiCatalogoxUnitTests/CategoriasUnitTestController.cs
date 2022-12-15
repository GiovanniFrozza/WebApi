using APICatalogo.Context;
using APICatalogo.Controllers;
using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Repository;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiCatalogoxUnitTests
{
    public class CategoriasUnitTestController
    {
        //Configuração do banco
        private IMapper mapper;
        private IUnitOfWork unitOfWork;

        public static DbContextOptions<AppDbContext> dbContextOptions { get; }

        public static string connectionString = "Server=localhost;DataBase=CatalogoDB;Uid=root;Pwd=admin";

        static CategoriasUnitTestController()
        {
            dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                .Options;
        }

        public CategoriasUnitTestController()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });

            mapper = config.CreateMapper();

            var context = new AppDbContext(dbContextOptions);

            unitOfWork = new UnitOfWork(context);
        }

        //Testes unitários

        [Fact]
        public void GetCategorias_Return_OkResult()
        {
            var controller = new CategoriasController(unitOfWork, mapper);

            var data = controller.();

            Assert.IsType<List<CategoriaDTO>>(data);
        }
    }
}
