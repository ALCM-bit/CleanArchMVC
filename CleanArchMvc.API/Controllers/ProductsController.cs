using CleanArchMvc.Application.DTOs;
using CleanArchMvc.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchMvc.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> Get()
    {
        var produtos = await _productService.GetProducts();

        if(produtos == null)
        {
            return NotFound("Products not found.");
        }

        return Ok(produtos);
    }

    [HttpGet("{id}", Name = "GetProduct")]
    public async Task<ActionResult<ProductDTO>> Get(int id)
    {
        var produto = await _productService.GetById(id);

        if (produto == null)
        {
            return NotFound("Product not found.");
        }

        return Ok(produto);
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] ProductDTO productDTO)
    {
        if (productDTO == null)
        {
            return BadRequest("Data Invalid");
        }

        await _productService.Add(productDTO); ;

        return new CreatedAtRouteResult("GetProduct", new {id = productDTO.Id}, productDTO);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Put(int id, [FromBody] ProductDTO productDTO)
    {
        if (id != productDTO.Id)
        {
            return BadRequest("Data Invalid");
        }

        if(productDTO == null)
        {
            return BadRequest("Data Invalid");
        }

        await _productService.Update(productDTO); 

        return Ok(productDTO);
    }


    [HttpDelete("{id}")]
    public async Task<ActionResult<ProductDTO>> Delete(int id)
    {
        var produto = await _productService.GetById(id);

        if (produto == null)
        {
            return NotFound("Product not found.");
        }

        await _productService.Remove(id);

        return Ok(produto);
    }
}
