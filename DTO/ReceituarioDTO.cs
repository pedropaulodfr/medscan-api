namespace authentication_jwt.DTO
{
    public class ReceituarioDTO
    {
        public long Id { get; set; }
        public int? Frequencia { get; set; }
        public string? Tempo { get; set; }
        public string? Periodo { get; set; }
        public int? Dose { get; set; }
        public long? UsuarioId { get; set; }
        public MedicamentoDTO? Medicamento { get; set; }
    }
}