using Xadrez.API.Domain.Exceptions;
using XadrezApi.Domain;
using XadrezApi.Dtos;
using XadrezApi.Xadrez;

namespace Xadrez.API.Services;
public class JogoDeXadrezService
{
    private PartidaDeXadrez _partida; // Remove readonly modifier

    public JogoDeXadrezService()
    {
        _partida = new PartidaDeXadrez();
    }

    public PartidaDto ObterPartida()
    {
        return new PartidaDto
        {
            Turno = _partida.turno,
            JogadorAtual = _partida.jogadorAtual.ToString(),
            Terminada = _partida.terminada,
            Xeque = _partida.xeque
        };
    }

    public bool PartidaFinalizada => _partida.terminada;

    public List<string> ObterMovimentosPossiveis(string origemStr)
    {
        try
        {
            if (string.IsNullOrEmpty(origemStr) || origemStr.Length != 2)
                throw new TabuleiroException("Posição inválida.");

            Posicao origem = new PosicaoXadrez(origemStr[0], int.Parse(origemStr[1].ToString())).toPosicao();
            _partida.validarPosicaoDeOrigem(origem);

            var matriz = _partida.tab.peca(origem).movimentosPossiveis();
            var movimentos = new List<string>();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (matriz[i, j])
                    {
                        movimentos.Add(new PosicaoXadrez((char)('a' + j), 8 - i).ToString());
                    }
                }
            }

            return movimentos;
        }
        catch (Exception ex)
        {
            throw new TabuleiroException("Erro ao processar a posição: " + ex.Message);
        }
    }

    public void RealizarJogada(string origemStr, string destinoStr)
    {
        char coluna = origemStr[0];
        int linha = int.Parse(origemStr[1].ToString());

        Posicao origem = new PosicaoXadrez(coluna, linha).toPosicao();

        _partida.validarPosicaoDeOrigem(origem);

        Posicao destino = new PosicaoXadrez(destinoStr[0], int.Parse(destinoStr[1].ToString())).toPosicao();

        _partida.validarPosicaoDeDestino(origem, destino);

        _partida.realizaJogada(origem, destino);
    }

    public string ObterTabuleiroFormatado()
    {
        var sb = new System.Text.StringBuilder();
        var tab = _partida.tab;

        for (int i = 0; i < tab.linhas; i++)
        {
            sb.Append(8 - i).Append(" ");
            for (int j = 0; j < tab.colunas; j++)
            {
                var p = tab.peca(i, j);
                string simbolo = p != null ? RepresentacaoUnicode(p) + " " : ".. ";
                sb.Append(simbolo);
            }
            sb.AppendLine();
        }

        // Letras maiúsculas fullwidth para colunas
        sb.Append("  ");
        foreach (var letra in "ABCDEFGH")
        {
            char fullwidth = (char)(letra - 'A' + 0xFF21);
            sb.Append(fullwidth).Append(' ');
        }
        sb.AppendLine();

        return sb.ToString();
    }

    public void ReiniciarPartida()
    {
        _partida = new PartidaDeXadrez(); // Cria uma nova partida do zero
    }

    // Representa as peças com símbolos unicode
    private string RepresentacaoUnicode(Peca peca)
    {
        var simbolo = peca.ToString().ToLower(); // ou use peca.tipo se tiver
        return peca.cor == Cor.Branca ? simbolo switch
        {
            "t" => "♖",
            "c" => "♘",
            "b" => "♗",
            "d" => "♕",
            "r" => "♔",
            "p" => "♙",
            _ => "?"
        }
        : simbolo switch
        {
            "t" => "♜",
            "c" => "♞",
            "b" => "♝",
            "d" => "♛",
            "r" => "♚",
            "p" => "♟",
            _ => "?"
        };
    }
}
