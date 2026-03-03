using System.ComponentModel.DataAnnotations;

namespace SupermercadoAPI.DTOs.Cajas
{
    public class CajaAsignarDto
    {
        // null = dejar la caja libre (sin cajero)
        public int? ID_Usuario_Cajero { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El saldo inicial no puede ser negativo.")]
        public decimal SaldoInicial { get; set; } = 0;
    }

    public class CajaResponseDto
    {
        public int     ID_Caja       { get; set; }
        public string  NombreCaja    { get; set; } = string.Empty;
        public string? NombreCajero  { get; set; }
        public int?    ID_Cajero     { get; set; }
        public decimal SaldoInicial  { get; set; }
        public decimal TotalEntradas { get; set; }
        public decimal TotalSalidas  { get; set; }
        public decimal SaldoActual   { get; set; }
        public bool    EstadoActiva  { get; set; }
        public bool    Disponible    { get; set; }
    }

    public class CajaEstadoDto
    {
        public bool EstadoActiva { get; set; }
    }
}
