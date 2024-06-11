
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using MongoDB.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MongoDB {

    public class PadreServices {
         
        private readonly IMongoCollection<Padre> _padresCollention;

        public PadreServices(IOptions<PadreDbSettings> PadreDbSettings) {
            
            var mongoClient = new MongoClient (
                PadreDbSettings.Value.ConnectionString
            );

            var MongoDatabase = mongoClient.GetDatabase(
                PadreDbSettings.Value.DatabaseName
            );

            _padresCollention = MongoDatabase.GetCollection<Padre>(
                PadreDbSettings.Value.CollectionName
            );
        }

        public async Task<List<Padre>> GetAsync() {
            return await _padresCollention.Find(padre => true).ToListAsync();
        }

        public async Task<Padre?> GetAsync (string id) {
          return  await _padresCollention.Find(padre => padre.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Padre newPadre) {
            await _padresCollention.InsertOneAsync(newPadre);
        }

      public class UpdateResponse
        {
            public string? Message { get; set; }
        }

  public async Task<UpdateResponse> UpdateAsync(string id, Padre updatedPadre) {
    Padre existingPadre;

    try {
        existingPadre = await _padresCollention.Find(padre => padre.Id == id).FirstOrDefaultAsync();
    } catch (Exception) {
        return new UpdateResponse { Message = "El ID proporcionado no es válido" };
    }

    if (existingPadre == null) {
        return new UpdateResponse { Message = "No se pudo actualizar porque el padre no existe" };
    }

    var update = Builders<Padre>.Update
        .Set(padre => padre.nombre, updatedPadre.nombre)
        .Set(padre => padre.tipoVivienda, updatedPadre.tipoVivienda)
        .Set(padre => padre.cantidadHijos, updatedPadre.cantidadHijos)
        .Set(padre => padre.recibeAyuda, updatedPadre.recibeAyuda)
        .Set(padre => padre.telefono, updatedPadre.telefono);

    await _padresCollention.UpdateOneAsync(padre => padre.Id == id, update);

    return new UpdateResponse { Message = "El padre se ha actualizado correctamente" };
}
        public async Task<UpdateResponse> DeleteAsync(string id) {
              Padre existingPadre;
                try {
                    existingPadre = await _padresCollention.Find(padre => padre.Id == id).FirstOrDefaultAsync();
                } catch (Exception) {
                    return new UpdateResponse { Message = "El ID proporcionado no es válido" };
                }

                if (existingPadre == null) {
                    return new UpdateResponse { Message = "No se pudo actualizar porque el padre no existe" };
                }
                await _padresCollention.DeleteOneAsync(padre => padre.Id == id);

                return new UpdateResponse { Message = "El padre se ha eliminado correctamente" };
        }
    }
}

