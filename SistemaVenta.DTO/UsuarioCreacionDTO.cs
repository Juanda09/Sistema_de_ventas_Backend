namespace SistemaVenta.DTO
{
    // Clase DTO para la creación de usuarios, excluyendo idUsuario
    public class UsuarioCreacionDTO
    {
        public string? NombreCompleto { get; set; }
        public string? Correo { get; set; }
        public int? IdRol { get; set; }
        public string? Clave { get; set; }
        public int? EsActivo { get; set; }
    }
}
