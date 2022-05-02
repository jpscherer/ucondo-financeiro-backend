using AutoMapper;
using Ucondo_Financeiro_Dominio;
using Ucondo_Financeiro_Dominio.DTO;
using Ucondo_Financeiro_Dominio.Entities;
using Ucondo_Financeiro_Dominio.Exceptions;
using Ucondo_Financeiro_Dominio.Interfaces.Repositories;
using Ucondo_Financeiro_Dominio.Interfaces.Services;

namespace Ucondo_Financeiro_Service.Services
{
    public class ContaRateioService : IContaRateioService
    {
        private readonly IContaRateioRepository _repository;
        private readonly IMapper _mapper;
        private static int MAIOR_CODIGO_PERMITIDO = 999;
        public ContaRateioService(IContaRateioRepository contaRateioRepository, IMapper mapper)
        {
            _repository = contaRateioRepository;
            _mapper = mapper;
        }

        #region CRUD
        public IEnumerable<ContaRateio> Listar(string pesquisa)
        {
            return _repository.GetAll().Where(w => w.Codigo.ToString().ToLower().Contains(pesquisa) || w.Nome.ToLower().Contains(pesquisa.ToLower()));
        }

        public IEnumerable<ContaRateio> ListarByContaPai(Guid idContaPai)
        {
            return _repository.GetAll().Where(w => w.ContaPaiId == idContaPai).OrderBy(ob => ob.Codigo);
        }

        public IEnumerable<ContaRateio> ListarContasRaiz()
        {
            return _repository.GetAll().Where(w => w.ContaPaiId == Guid.Empty).OrderBy(ob => ob.Codigo);
        }

        public void Inserir(ContaRateio contaRateio)
        {
            RegrasNegocioInsercao(contaRateio);
            _repository.Inserir(contaRateio);
        }

        private void RegrasNegocioInsercao(ContaRateio contaRateio)
        {
            ValidarCodigoRepetido(contaRateio);
            if (contaRateio.ContaPaiId.HasValue && contaRateio.ContaPaiId != Guid.Empty)
            {
                var contaPai = _repository.GetAll().Single(s => s.Id == contaRateio.ContaPaiId);
                ValidarMesmoTipoContaPai(contaRateio.Tipo, contaPai.Tipo);
                ValidarContaPaiAceitaLancamentos(contaPai.AceitaLancamentos);
            }
        }

        private void ValidarContaPaiAceitaLancamentos(bool contaPaiAceitaLancamentos)
        {
            if (contaPaiAceitaLancamentos)
                throw new NegocioFinanceiroException(MensagensDeUsuario.NAO_E_PERMITIDO_CADASTRAR_CONTA_FILHA_QUANDO_CONTA_PAI_ACEITA_LANCAMENTOS);
        }

        private void ValidarMesmoTipoContaPai(int tipoContaFilha, int tipoContaPai)
        {
            if (tipoContaFilha != tipoContaPai)
                throw new NegocioFinanceiroException(MensagensDeUsuario.NAO_E_PERMITIDO_CONTA_FILHA_TIPO_DIFERENTE_CONTA_PAI);
        }

        public void Alterar(ContaRateio contaRateio)
        {
            RegrasNegocioAlteracao(contaRateio);
            _repository.Alterar(contaRateio);
        }

        private void RegrasNegocioAlteracao(ContaRateio contaRateio)
        {
            var contaAnterior = _repository.GetAll().Single(s => s.Id == contaRateio.Id);
            var possuiFilhas = PossuiContasFilhas(contaRateio.Id);

            ValidarCodigoRepetido(contaRateio);
            ValidarAlteracaoTipoConta(contaRateio.Tipo, contaAnterior.Tipo, possuiFilhas);
            ValidarAceitaLancamentos(contaRateio.AceitaLancamentos, possuiFilhas);
        }

        private void ValidarCodigoRepetido(ContaRateio contaRateio)
        {
            var codigoJaCadastrado = _repository.GetAll().Any(a => a.Codigo == contaRateio.Codigo && a.ContaPaiId == contaRateio.ContaPaiId && a.Id != contaRateio.Id);
            if (codigoJaCadastrado)
                throw new NegocioFinanceiroException(MensagensDeUsuario.CODIGO_JA_CADASTRADO);
        }

        private void ValidarAceitaLancamentos(bool contaAceitaLancamentos, bool possuiFilhas)
        {
            if (contaAceitaLancamentos && possuiFilhas)
                throw new NegocioFinanceiroException(MensagensDeUsuario.NAO_ACEITA_LANCAMENTOS_QUANDO_POSSUI_FILHAS);
        }

        private void ValidarAlteracaoTipoConta(int tipoAtual, int tipoAnterior, bool possuiFilhas)
        {
            if (tipoAtual != tipoAnterior && possuiFilhas)
                throw new NegocioFinanceiroException(MensagensDeUsuario.NAO_E_POSSIVEL_ALTERAR_TIPO_CONTA_COM_FILHAS);
        }

        public void Deletar(Guid id)
        {
            RegrasNegocioExclusao(id);
            _repository.Deletar(id);
        }

        private void RegrasNegocioExclusao(Guid id)
        {
            if (PossuiContasFilhas(id))
                throw new NegocioFinanceiroException(MensagensDeUsuario.CONTA_POSSUI_REGISTROS_FILHOS);
        }

        private bool PossuiContasFilhas(Guid id)
        {
            return _repository.GetAll().Any(a => a.ContaPaiId == id);
        }
        #endregion

        #region Proximo Codigo
        public ProximoCodigoContaRateioDTO ProximoCodigo(Guid? idContaPai)
        {
            if (!idContaPai.HasValue || idContaPai.Value == Guid.Empty)
                return ProximoCodigoContaRaiz();

            return ProximoCodigoContaIrma(idContaPai.Value);
        }
        private ProximoCodigoContaRateioDTO ProximoCodigoContaRaiz()
        {
            var maiorNumeroUtilizado = _repository.GetAll().Max(x => x.Codigo);

            if (maiorNumeroUtilizado == MAIOR_CODIGO_PERMITIDO)
                throw new NegocioFinanceiroException(MensagensDeUsuario.MAIOR_CODIGO_DE_CONTA_UTILIZADO);

            return new ProximoCodigoContaRateioDTO()
            {
                Codigo = ++maiorNumeroUtilizado,
                ContaPaiAlterada = false,
                IdContaPai = Guid.Empty
            };
        }
        private ProximoCodigoContaRateioDTO ProximoCodigoContaIrma(Guid idContaPai)
        {
            if (!_repository.GetAll().Any(w => w.ContaPaiId == idContaPai))
                return RetornarPrimeiroCodigoComoContaPai(idContaPai);

            var maiorNumeroUtilizadoPelasIrmas = _repository.GetAll().Where(w => w.ContaPaiId == idContaPai).Max(x => x.Codigo);

            if (maiorNumeroUtilizadoPelasIrmas == MAIOR_CODIGO_PERMITIDO)
                return ProximoCodigoContaPai(idContaPai);

            return new ProximoCodigoContaRateioDTO()
            {
                Codigo = ++maiorNumeroUtilizadoPelasIrmas,
                ContaPaiAlterada = false,
                IdContaPai = idContaPai
            };
        }
        private ProximoCodigoContaRateioDTO ProximoCodigoContaPai(Guid idContaPai)
        {
            var idContaAvo = _repository.GetAll().Single(w => w.Id == idContaPai).ContaPaiId;
            var maiorNumeroUtilizadoPelasTias = _repository.GetAll().Where(w => w.ContaPaiId == idContaAvo).Max(x => x.Codigo);

            if (maiorNumeroUtilizadoPelasTias == MAIOR_CODIGO_PERMITIDO)
            {
                if (idContaAvo == null)
                    throw new NegocioFinanceiroException(MensagensDeUsuario.CODIGO_MAXIMO_UTILIZADO_TODOS_NIVEIS);

                return ProximoCodigoContaPai(idContaAvo.Value);
            }

            return new ProximoCodigoContaRateioDTO()
            {
                Codigo = ++maiorNumeroUtilizadoPelasTias,
                ContaPaiAlterada = true,
                IdContaPai = (idContaAvo.HasValue ? idContaAvo.Value : Guid.Empty)
            };
        }

        private ProximoCodigoContaRateioDTO RetornarPrimeiroCodigoComoContaPai(Guid idContaPai)
        {
            return new ProximoCodigoContaRateioDTO()
            {
                Codigo = 1,
                ContaPaiAlterada = false,
                IdContaPai = idContaPai
            };
        }
        #endregion Proximo Codigo
    }
}
