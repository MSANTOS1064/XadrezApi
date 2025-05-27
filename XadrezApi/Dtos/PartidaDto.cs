namespace XadrezApi.Dtos
{
    public class PartidaDto
    {
        public int Turno { get; set; }
        public string JogadorAtual { get; set; }
        public bool Terminada { get; set; }
        public bool Xeque { get; set; }
    }

}
