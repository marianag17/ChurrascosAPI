namespace ChurrascosAPI.Models.Enums
{
    public enum TipoCarne
    {
        Puyazo = 0,
        Culotte = 1,
        Costilla = 2
    }

    public enum TerminoCoccion
    {
        TerminMedio = 0,
        TerminTresCuartos = 1,
        BienCocido = 2
    }

    public enum TipoPlato
    {
        Individual = 0,
        Familiar3Porciones = 1,
        Familiar5Porciones = 2
    }

    public enum TipoDulce
    {
        CanillitasLeche = 0,
        Pepitoria = 1,
        Cocadas = 2,
        DulcesHigo = 3,
        Mazapanes = 4,
        Chilacayotes = 5,
        ConservasCoco = 6,
        ColochosGuayaba = 7
    }

    public enum ModalidadVenta
    {
        Unidad = 0,
        Caja6 = 1,
        Caja12 = 2,
        Caja24 = 3
    }

    public enum TipoCombo
    {
        Familiar = 0,
        Eventos = 1,
        Personalizado = 2
    }

    public enum TipoInventario
    {
        Carne = 0,
        Guarnicion = 1,
        Dulce = 2,
        Empaque = 3,
        Combustible = 4
    }

    public enum TipoVenta
    {
        Local = 0,
        Domicilio = 1,
        Eventos = 2
    }

    public enum EstadoVenta
    {
        Pendiente = 0,
        Preparando = 1,
        Listo = 2,
        Entregado = 3,
        Cancelado = 4
    }
}