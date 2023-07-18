using Store.Domain.Commands;
using Store.Domain.Commands.Interfaces;
using Store.Domain.Handlers;
using Store.Domain.Repositories;
using Store.Tests.Repositories;

namespace Store.Tests.Handlers;

[TestClass]
public class OrderHandlerTests
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IDeliveryFeeRepository _deliveryFeeRepository;
    private readonly IDiscountRepository _discountRepository;
    private readonly IProductRepository _productRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly CreateOrderCommand _command = new CreateOrderCommand();
    private readonly OrderHandler _handler;

    public OrderHandlerTests()
    {
        _customerRepository = new FakeCustomerRepository();
        _deliveryFeeRepository = new FakeDeliveryFeeRepository();
        _discountRepository = new FakeDiscountRepository();
        _productRepository = new FakeProductRepository();
        _orderRepository = new FakeOrderRepository();
        
        _handler = new OrderHandler(
            _customerRepository,
            _deliveryFeeRepository,
            _discountRepository,
            _productRepository,
            _orderRepository
        );

        _command.Customer = "12345678911";
        _command.ZipCode = "12345678";
        _command.PromoCode = "12345678";
        _command.Items.Add(new CreateOrderItemCommand(Guid.NewGuid(), 1));
        _command.Items.Add(new CreateOrderItemCommand(Guid.NewGuid(), 1));
    }    

    [TestMethod]
    [TestCategory("Handlers")]
    public void Dado_um_cliente_inexistente_o_pedido_nao_deve_ser_gerado()
    {
        _command.Customer = null;
        _handler.Handle(_command);

        Assert.AreEqual(_command.IsValid, false);
    }

    [TestMethod]
    [TestCategory("Handlers")]
    public void Dado_um_cep_invalido_o_pedido_nao_deve_ser_gerado()
    {
        _command.ZipCode = "1234567";
        _handler.Handle(_command);
        
        Assert.AreEqual(_command.IsValid, false);
    }

    [TestMethod]
    [TestCategory("Handlers")]
    public void Dado_um_promocode_inexistente_o_pedido_deve_ser_gerado_normalmente()
    {
        _command.PromoCode = null;
        _handler.Handle(_command);
        
        Assert.AreEqual(_command.IsValid, true);
    }

    [TestMethod]
    [TestCategory("Handlers")]
    public void Dado_um_pedido_sem_items_o_mesmo_nao_deve_ser_gerado()
    {
        _command.Items.Clear();
        _handler.Handle(_command);
        
        Assert.AreEqual(_command.IsValid, false);
    }

    [TestMethod]
    [TestCategory("Handlers")]
    public void Dado_um_comando_ivalido_o_pedido_nao_deve_ser_gerado()
    {
        _command.Customer = "";
        _handler.Handle(_command);

        Assert.AreEqual(_command.IsValid, false);
    }

    [TestMethod]
    [TestCategory("Handlers")]
    public void Dado_um_comando_valido_o_pedido_deve_ser_gerado()
    {
        _handler.Handle(_command);

        Assert.AreEqual(_command.IsValid, true);
    }
}