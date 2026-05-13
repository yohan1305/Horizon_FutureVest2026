
namespace Application.Dtos.RankingGeneral
{
    public class RankingGeneralDto
    {
        public string NombrePais { get; set; }           // Nombre del país
        public string CodigoIso { get; set; }            // Código ISO del país
        public decimal Scoring { get; set; }             // Puntaje total (entre 0 y 1)
        public decimal TasaRetorno { get; set; }         // Tasa estimada de retorno (%)
        public int Posicion { get; set; }

    }
}
