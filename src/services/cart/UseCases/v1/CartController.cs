using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VND.CoolStore.Services.Cart.Domain;
using VND.CoolStore.Services.Cart.Infrastructure.Service;
using VND.CoolStore.Shared.Cart.Checkout;
using VND.CoolStore.Shared.Cart.GetCartById;
using VND.CoolStore.Shared.Cart.InsertItemToNewCart;
using VND.CoolStore.Shared.Cart.UpdateItemInCart;

namespace VND.CoolStore.Services.Cart.UseCases.v1
{
  [ApiVersion("1.0")]
  [Route("api/v{api-version:apiVersion}/carts")]
  public class CartController : FW.Infrastructure.AspNetCore.ControllerBase
  {
    private readonly ICartService _cartService;

    public CartController(ICartService cartService, NoTaxCaculator taxCaculator)
    {
      _cartService = cartService;
      _cartService.PriceCalculatorContext = taxCaculator;
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<GetCartByIdResponse> Get(Guid id)
    {
      //TODO: stupid code
      if (id == Guid.Empty)
        return null;

      return await _cartService.GetCartByIdAsync(id);
    }

    [HttpPost]
    public async Task<GetCartByIdResponse> Create([FromBody] InsertItemToNewCartRequest request)
    {
      var cart = await _cartService.InsertItemToCartAsync(request);
      return await _cartService.GetCartByIdAsync(cart.Id);
    }

    [HttpPut]
    public async Task<GetCartByIdResponse> Put([FromBody] UpdateItemInCartRequest request)
    {
      var cart = await _cartService.UpdateItemInCartAsync(request);
      return await _cartService.GetCartByIdAsync(cart.Id);
    }

    [HttpDelete]
    [Route("{cartId:guid}/items/{productId:guid}")]
    public async Task<bool> RemoveItemInCart(Guid cartId, Guid productId)
    {
      return await _cartService.RemoveItemInCartAsync(cartId, productId);
    }

    [HttpPost]
    [Route("{cartId:guid}/checkout")]
    public async Task<CheckoutResponse> CheckoutCart(Guid cartId)
    {
      return await _cartService.CheckoutAsync(new CheckoutRequest { Id = cartId });
    }
  }
}
