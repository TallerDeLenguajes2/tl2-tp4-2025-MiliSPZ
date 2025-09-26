namespace SistemaCatederia;

public class Cliente
{
    private string? referencia;

    /* Relaciones */
    /* - */

    /* Campos */
    public string? id { get; set; }
    public string? nombre { get; set; }
    public string? direccion { get; set; }
    public string? telefono { get; set; }
    public string? datosReferenciaDireccion { get; set; }
    public Cliente? ClienteUnico { get; }

    /* Constructores */
    public Cliente() { }

    public Cliente(Cliente? cliente)
    {
        ClienteUnico = cliente;
    }

    public Cliente(string? nombre, string? direccion, string? telefono, string? datosReferenciaDireccion, Cliente? clienteUnico)
    {
        this.nombre = nombre;
        this.direccion = direccion;
        this.telefono = telefono;
        this.datosReferenciaDireccion = datosReferenciaDireccion;
        ClienteUnico = clienteUnico;
    }

    public Cliente(string? nombre, string? direccion, string? telefono, string? referencia)
    {
        this.nombre = nombre;
        this.direccion = direccion;
        this.telefono = telefono;
        this.referencia = referencia;
    }

    /* Metodos */
}