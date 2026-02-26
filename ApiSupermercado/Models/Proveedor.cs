using System.ComponentModel.DataAnnotations;

namespace SupermercadoAPI.Models
{
    public class Proveedor
    {
        [Key]
        public int ID_Proveedor { get; set; }
        public string NombreEmpresa { get; set; } = string.Empty;
        public string? ContactoAsignado { get; set; }
        public string? Telefono { get; set; }
        public bool EstadoActivo { get; set; } = true;

        public ICollection<InventarioLote> Lotes { get; set; } = new List<InventarioLote>();
    }
}
