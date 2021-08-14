using System;
using APIInfectados.Data.Collections;
using APIInfectados.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace APIInfectados.Controllers
{
    [ApiController]
    [Route("[controller]")]
    // [Route("api/V1/[controller]")]
    // [ApiController]
    public class InfectadoController : ControllerBase
    {
        Data.MongoDB _mongoDB;
        IMongoCollection<Infectado> _infectadosCollection;

        public InfectadoController(Data.MongoDB mongoDB)
        {
            _mongoDB = mongoDB;
            _infectadosCollection = _mongoDB.DB.GetCollection<Infectado>(typeof(Infectado).Name.ToLower());
        }

        [HttpPost]
        public ActionResult SalvarInfectado([FromBody] InfectadoDto dto)
        {
            var infectado = new Infectado(dto.DataNascimento, dto.Sexo, dto.Latitude, dto.Longitude);

            _infectadosCollection.InsertOne(infectado);
            
            return StatusCode(201, "Infectado adicionado com sucesso");
        }

        [HttpGet]
        public ActionResult ObterInfectados()
        {
            var infectados = _infectadosCollection.Find(Builders<Infectado>.Filter.Empty).ToList();
            
            return Ok(infectados);
        }

        [HttpPut]
        public ActionResult AtualizarInfectado([FromBody] InfectadoDto dto)
        {
            _infectadosCollection.UpdateOne(Builders<Infectado>.Filter.Where(inf => inf.DataNascimento == dto.DataNascimento), Builders<Infectado>.Update.Set("sexo", dto.Sexo));

            return Ok("Atualizo com sucesso!");
        }

        [HttpDelete("{dataNascimento}")]
        public ActionResult Delete(DateTime dataNascimento)
        {
            _infectadosCollection.DeleteOne(Builders<Infectado>.Filter.Where(inf => inf.DataNascimento == dataNascimento));

            return Ok("Deletado com sucesso!");
            
        }
    }
}
