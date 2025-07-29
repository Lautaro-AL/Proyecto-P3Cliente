namespace ClienteObligatorioP3.Models
{
    public class DTOEnvio
    {
        public int? NumTracking { get; set; }
        public string? tipoEnvio { get; set; }
        public int? EnvioID { get; set; }
        public int? ClienteId { get; set; }
        public int? Peso { get; set; }
        public List<DTOSeguimiento> Seguimiento { get; set; }
        public string? Estado { get; set; }
        public DTOAgencias? AgenciaEnvio { get; set; }
        public string? Direccion { get; set; }
        public bool? valorDeEntrega { get; set; }
        public DateTime? FechaFinalizacion { get; set; }
    }
}
