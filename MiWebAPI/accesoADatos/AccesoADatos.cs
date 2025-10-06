using System.Text.Json;

namespace SistemaCatederia;

public class AccesoADatos
{
    private readonly string rutaCadeteriaCVS = "Archivos/Cadeteria.csv";
    private readonly string rutaCadetesCVS = "Archivos/Cadetes.csv";
    private readonly string rutaCadeteriaJSON = "Archivos/Cadeteria.json";


    public Catederia TraerCadeteriaCVS()
    {
        var cadeteria = new Catederia("Sin nombre", "381-9999999", new List<Cadete>());

        if (!File.Exists(rutaCadeteriaCVS))
        {
            return cadeteria;
        }

        var lineas = File.ReadAllLines(rutaCadeteriaCVS);
        foreach (var linea in lineas)
        {
            var partes = linea.Split(",");
            if (partes.Length == 2)
            {
                cadeteria.nombre = partes[0];
                cadeteria.telefono = partes[1];
            }
        }

        return cadeteria;
    }

    public List<Cadete> TraerCadetesCVS()
    {
        var cadetes = new List<Cadete>();

        if (!File.Exists(rutaCadetesCVS))
        {
            return cadetes;
        }

        var lineas = File.ReadAllLines(rutaCadetesCVS);
        foreach (var linea in lineas)
        {
            var partes = linea.Split(",");
            if (partes.Length >= 2 && int.TryParse(partes[0], out int id))
            {
                cadetes.Add(new Cadete(id, partes[1], new List<Pedido>()));
            }
        }
        return cadetes;
    }

    public Catederia TraerCadeteriaJSON()
    {
        if (!File.Exists(rutaCadeteriaJSON))
        {
            return new Catederia("Sin datos", "381-9999999", new List<Cadete>());
        }

        string json = File.ReadAllText(rutaCadeteriaJSON);
        if (string.IsNullOrWhiteSpace(json))
        {
            return new Catederia("Sin datos", "381-9999999", new List<Cadete>());
        }

        var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        return JsonSerializer.Deserialize<Catederia>(json, opciones) ?? new Catederia("Error", "381-9999999", new List<Cadete>());
    }

    public void GuardarCadeteriaJson(Catederia catederia)
    {
        var opciones = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(catederia, opciones);
        File.WriteAllText(rutaCadeteriaJSON, json);
    }
}