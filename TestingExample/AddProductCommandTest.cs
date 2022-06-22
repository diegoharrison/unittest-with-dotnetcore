using LiveTestingExemplo.Commands.AddProduct;
using LiveTestingExemplo.Entities;
using LiveTestingExemplo.Repositories;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace TestingExample
{
    public class AddProductCommandTest
    {
        private readonly Mock<IProductRepository> _productRepository;
        private readonly AddProductCommandHandler _addProductCommandHandler;

        public AddProductCommandTest()
        {
            _productRepository = new Mock<IProductRepository>();
            _addProductCommandHandler = new AddProductCommandHandler(
                _productRepository.Object
            );
        }      

        [Fact]
        public async Task CommandIsValid_Executed_ShouldSuccess()
        {
            //Arrange
            var addProductCommand = new AddProductCommand("Title", "Description", 1000);
            var product = new Product("", "", 1000);

            _productRepository.Setup(p => p.Add(It.IsAny<Product>())).ReturnsAsync(product); // usar o ReturnsAsync ou Returns quando tiver retorno.  
                                                                                             // usar o Verifiable quando o método não tiver retorno, somente pra verificação.  
            //Act
            var productResult = await _addProductCommandHandler.Handle(addProductCommand, new CancellationToken());

            //Assert
            _productRepository.Verify(p => p.Add(It.IsAny<Product>()), Times.Once);
            Assert.NotNull(productResult);
            Assert.Equal(product.Price, productResult.Price);
            Assert.Equal(product.Title, productResult.Title);
            Assert.Equal(product.Description, productResult.Description);
        }
    }
}