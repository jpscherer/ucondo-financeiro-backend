using AutoMapper;
using NSubstitute;
using Ucondo_Financeiro_Dominio;
using Ucondo_Financeiro_Dominio.Entities;
using Ucondo_Financeiro_Dominio.Exceptions;
using Ucondo_Financeiro_Dominio.Interfaces.Repositories;
using Ucondo_Financeiro_Service.Services;
using Ucondo_Financeiro_Testes.Builders;
using Xunit;

namespace Ucondo_Financeiro_Testes.Service
{
    public class ContaRateioServiceTest
    {
        private readonly ContaRateioService _service;
        private readonly IContaRateioRepository _repository;
        private readonly IMapper _mapper;

        private static int _maiorCodigoPermitido = 999;

        public ContaRateioServiceTest()
        {
            _repository = Substitute.For<IContaRateioRepository>();
            _mapper = Substitute.For<IMapper>();

            _service = new ContaRateioService(_repository, _mapper);
        }

        #region CRUD
        [Fact]
        public void Nao_Permitir_Alterar_Tipo_Quando_Possui_Filha()
        {
            var contaPai = new ContaRateioBuilder().ComId(Guid.NewGuid()).ComCodigo(10).ComTipo(1).ComLancamento(false).Build();
            var contaPaiClone = new ContaRateioBuilder().ComId(contaPai.Id).ComCodigo(10).ComTipo(1).ComLancamento(false).Build();
            var contaRateio = new ContaRateioBuilder().ComId(Guid.NewGuid()).ComCodigo(10).ComContaPai(contaPai.Id).ComTipo(1).ComLancamento(true).Build();
            
            _repository.GetAll().Returns(new List<ContaRateio>() { contaPai, contaRateio }.AsEnumerable());

            contaPaiClone.Tipo = contaPai.Tipo+1;

            var ex = Assert.Throws<NegocioFinanceiroException>(() => _service.Alterar(contaPaiClone));
            Assert.Equal(MensagensDeUsuario.NAO_E_POSSIVEL_ALTERAR_TIPO_CONTA_COM_FILHAS, ex.Message);
        }

        [Fact]
        public void Nao_Permitir_Alterar_Lancamento_Quando_Possui_Filha()
        {
            var contaPai = new ContaRateioBuilder().ComId(Guid.NewGuid()).ComCodigo(10).ComTipo(1).ComLancamento(false).Build();
            var contaRateio = new ContaRateioBuilder().ComId(Guid.NewGuid()).ComCodigo(10).ComContaPai(contaPai.Id).ComTipo(1).ComLancamento(true).Build();

            _repository.GetAll().Returns(new List<ContaRateio>() { contaPai, contaRateio }.AsEnumerable());

            contaPai.AceitaLancamentos = true;

            var ex = Assert.Throws<NegocioFinanceiroException>(() => _service.Alterar(contaPai));
            Assert.Equal(MensagensDeUsuario.NAO_ACEITA_LANCAMENTOS_QUANDO_POSSUI_FILHAS, ex.Message);

        }


        [Fact]
        public void Deve_Validar_Novo_Codigo_Repetido()
        {
            var contaRateioExistente = new ContaRateioBuilder().ComId(Guid.NewGuid()).ComCodigo(10).ComTipo(1).Build();
            var contaCodigoRepetido = new ContaRateioBuilder().ComId(Guid.NewGuid()).ComCodigo(11).ComTipo(1).Build();

            _repository.GetAll().Returns(new List<ContaRateio>() { contaRateioExistente, contaCodigoRepetido }.AsEnumerable());

            contaRateioExistente.Codigo = contaCodigoRepetido.Codigo;

            var ex = Assert.Throws<NegocioFinanceiroException>(() => _service.Alterar(contaRateioExistente));
            Assert.Equal(MensagensDeUsuario.CODIGO_JA_CADASTRADO, ex.Message);
        }

        [Fact]
        public void Deve_Alterar_Conta()
        {
            var contaRateioExistente = new ContaRateioBuilder().ComId(Guid.NewGuid()).ComCodigo(10).ComTipo(1).Build();
            _repository.GetAll().Returns(new List<ContaRateio>() { contaRateioExistente }.AsEnumerable());

            contaRateioExistente.Codigo = 11;

            _service.Alterar(contaRateioExistente);
            _repository.Received(1).Alterar(contaRateioExistente);
        }

        [Fact]
        public void Deve_Permitir_Codigo_Menor_Mesmo_Pai()
        {
            var contaPai = new ContaRateioBuilder().ComId(Guid.NewGuid()).ComCodigo(10).ComTipo(1).Build();
            var contaRateioExistente = new ContaRateioBuilder().ComId(Guid.NewGuid()).ComCodigo(10).ComContaPai(contaPai.Id).ComTipo(1).Build();
            var contaRateio = new ContaRateioBuilder().ComId(Guid.NewGuid()).ComCodigo(5).ComContaPai(contaPai.Id).ComTipo(1).Build();

            _repository.GetAll().Returns(new List<ContaRateio>() { contaPai, contaRateioExistente }.AsEnumerable());

            _service.Inserir(contaRateio);

            _repository.Received(1).Inserir(contaRateio);
        }

        [Fact]
        public void Deve_Validar_Conta_Pai_Aceita_Lancamentos()
        {
            var contaPai = new ContaRateioBuilder().ComId(Guid.NewGuid()).ComCodigo(10).ComTipo(1).ComLancamento(true).Build();
            var contaRateio = new ContaRateioBuilder().ComId(Guid.NewGuid()).ComCodigo(10).ComContaPai(contaPai.Id).ComTipo(1).Build();

            _repository.GetAll().Returns(new List<ContaRateio>() { contaPai }.AsEnumerable());

            var ex = Assert.Throws<NegocioFinanceiroException>(() => _service.Inserir(contaRateio));
            Assert.Equal(MensagensDeUsuario.NAO_E_PERMITIDO_CADASTRAR_CONTA_FILHA_QUANDO_CONTA_PAI_ACEITA_LANCAMENTOS, ex.Message);
        }

        [Fact]
        public void Deve_Validar_Conta_Filha_Mesmo_Tipo_Pai()
        {
            var contaPai = new ContaRateioBuilder().ComId(Guid.NewGuid()).ComCodigo(10).ComTipo(1).Build();
            var contaRateio = new ContaRateioBuilder().ComId(Guid.NewGuid()).ComCodigo(10).ComContaPai(contaPai.Id).ComTipo(2).Build();

            _repository.GetAll().Returns(new List<ContaRateio>() { contaPai }.AsEnumerable());

            var ex = Assert.Throws<NegocioFinanceiroException>(() => _service.Inserir(contaRateio));
            Assert.Equal(MensagensDeUsuario.NAO_E_PERMITIDO_CONTA_FILHA_TIPO_DIFERENTE_CONTA_PAI, ex.Message);
        }

        [Fact]
        public void Deve_Validar_Codigo_Repetido()
        {
            var contaRateio = new ContaRateioBuilder().ComId(Guid.NewGuid()).ComCodigo(10).ComNome("teste conta 1").Build();
            var contaRateioNova = new ContaRateioBuilder().ComId(Guid.NewGuid()).ComCodigo(10).ComNome("teste conta 2").Build();

            _repository.GetAll().Returns(new List<ContaRateio>() { contaRateio }.AsEnumerable());

            var ex = Assert.Throws<NegocioFinanceiroException>(() => _service.Inserir(contaRateioNova));
            Assert.Equal(MensagensDeUsuario.CODIGO_JA_CADASTRADO, ex.Message);
        }

        [Fact]
        public void Deve_Inserir_Conta_Nova()
        {
            var contaRateio = new ContaRateioBuilder().ComId(Guid.NewGuid()).ComCodigo(10).ComNome("teste conta").Build();
            _service.Inserir(contaRateio);
            _repository.Received(1).Inserir(contaRateio);
        }

        [Fact]
        public void Nao_Deve_Excluir_com_Conta_Filha()
        {
            var contaPai = new ContaRateioBuilder().ComId(Guid.NewGuid()).ComCodigo(10).Build();
            var contaRateio = new ContaRateioBuilder().ComId(Guid.NewGuid()).ComCodigo(10).ComContaPai(contaPai.Id).Build();

            _repository.GetAll().Returns(new List<ContaRateio>() { contaPai, contaRateio }.AsEnumerable());

            var ex = Assert.Throws<NegocioFinanceiroException>(() => _service.Deletar(contaPai.Id));
            Assert.Equal(MensagensDeUsuario.CONTA_POSSUI_REGISTROS_FILHOS, ex.Message);
        }

        [Fact]
        public void Deve_Excluir_Registro()
        {
            var contaPai = new ContaRateioBuilder().ComId(Guid.NewGuid()).ComCodigo(10).Build();
            var contaRateio = new ContaRateioBuilder().ComId(Guid.NewGuid()).ComCodigo(10).ComContaPai(contaPai.Id).Build();

            _repository.GetAll().Returns(new List<ContaRateio>() { contaPai, contaRateio }.AsEnumerable());

            _service.Deletar(contaRateio.Id);

            _repository.Received(1).Deletar(contaRateio.Id);
        }

        #endregion CRUD

        #region Proximo Codigo
        [Fact]
        public void Quando_Pai_Passivel_Incrementar()
        {
            //Atual: 999.10.(sugestao pra aq)
            //Maior do mesmo nível 999
            //Esperado: 999.11
            var contaAvo = new ContaRateioBuilder().ComId(Guid.NewGuid()).ComCodigo(999).Build();
            var contaPai = new ContaRateioBuilder().ComId(Guid.NewGuid()).ComContaPai(contaAvo.Id).ComCodigo(10).Build();
            var contaIrma1 = new ContaRateioBuilder().ComId(Guid.NewGuid()).ComContaPai(contaPai.Id).ComCodigo(_maiorCodigoPermitido).Build();
            var contaSugestao = new ContaRateioBuilder().ComContaPai(contaPai.Id).Build();

            _repository.GetAll().Returns(new List<ContaRateio>() { contaAvo, contaPai, contaIrma1 }.AsEnumerable());

            var retorno = _service.ProximoCodigo(contaSugestao.ContaPaiId);

            Assert.Equal(contaAvo.Id, retorno.IdContaPai);
            Assert.True(retorno.ContaPaiAlterada);
            Assert.Equal(contaPai.Codigo + 1, retorno.Codigo);
        }

        [Fact]
        public void Quando_Avo_Passivel_Incrementar()
        {
            //Atual: 10.999.(sugestao pra aq)
            //Maior do mesmo nível 999
            //Esperado: 11
            var contaAvo = new ContaRateioBuilder().ComId(Guid.NewGuid()).ComCodigo(10).Build();
            var contaPai = new ContaRateioBuilder().ComId(Guid.NewGuid()).ComContaPai(contaAvo.Id).ComCodigo(_maiorCodigoPermitido).Build();
            var contaIrma1 = new ContaRateioBuilder().ComId(Guid.NewGuid()).ComContaPai(contaPai.Id).ComCodigo(_maiorCodigoPermitido).Build();
            var contaSugestao = new ContaRateioBuilder().ComContaPai(contaPai.Id).Build();

            _repository.GetAll().Returns(new List<ContaRateio>() { contaAvo, contaPai, contaIrma1 }.AsEnumerable());

            var retorno = _service.ProximoCodigo(contaSugestao.ContaPaiId);

            Assert.Equal(Guid.Empty, retorno.IdContaPai);
            Assert.True(retorno.ContaPaiAlterada);
            Assert.Equal(contaAvo.Codigo + 1, retorno.Codigo);
        }

        [Fact]
        public void Validar_Todos_Niveis_Maior_Codigo()
        {
            //999.999.(sugestao pra aq)
            //Maior do mesmo nível 999
            //Esperado: Alerta todos máximo utilizados
            var contaAvo = new ContaRateioBuilder().ComId(Guid.NewGuid()).ComCodigo(_maiorCodigoPermitido).Build();
            var contaPai = new ContaRateioBuilder().ComId(Guid.NewGuid()).ComContaPai(contaAvo.Id).ComCodigo(_maiorCodigoPermitido).Build();
            var contaIrma1 = new ContaRateioBuilder().ComId(Guid.NewGuid()).ComContaPai(contaPai.Id).ComCodigo(_maiorCodigoPermitido).Build();
            var contaSugestao = new ContaRateioBuilder().ComContaPai(contaPai.Id).Build();

            _repository.GetAll().Returns(new List<ContaRateio>() { contaAvo, contaPai, contaIrma1 }.AsEnumerable());

            var ex = Assert.Throws<NegocioFinanceiroException>(() => _service.ProximoCodigo(contaSugestao.ContaPaiId));
            Assert.Equal(MensagensDeUsuario.CODIGO_MAXIMO_UTILIZADO_TODOS_NIVEIS, ex.Message);
        }

        [Fact]
        public void Validar_Codigo_Maximo_Contas_Irmas()
        {
            //20.(sugestao pra aq)
            //Maior do mesmo nível 21
            //Esperado: 20.22
            var contaPai = new ContaRateioBuilder().ComId(Guid.NewGuid()).ComCodigo(20).Build();
            var contaIrma1 = new ContaRateioBuilder().ComCodigo(20).ComContaPai(contaPai.Id).Build();
            var contaIrmaMaiorCodigo = new ContaRateioBuilder().ComCodigo(21).ComContaPai(contaPai.Id).Build();
            var contaIrma3 = new ContaRateioBuilder().ComCodigo(15).ComContaPai(contaPai.Id).Build();

            _repository.GetAll().Returns(new List<ContaRateio>() { contaPai, contaIrma1, contaIrmaMaiorCodigo, contaIrma3 }.AsEnumerable());

            var contaSugestao = new ContaRateioBuilder().ComContaPai(contaPai.Id).Build();
            var retorno = _service.ProximoCodigo(contaSugestao.ContaPaiId);

            Assert.False(retorno.ContaPaiAlterada);
            Assert.Equal(contaIrmaMaiorCodigo.Codigo + 1, retorno.Codigo);
        }

        [Fact]
        public void Retornar_Proximo_Codigo_Valido_Conta_Raiz()
        {
            //(sugestao pra aq)
            //Maior do mesmo nível 10
            //Esperado: 11
            var contaRaizMaiorCodigo = new ContaRateioBuilder().ComId(Guid.NewGuid()).ComCodigo(10).Build();
            var outraContaRaiz = new ContaRateioBuilder().ComId(Guid.NewGuid()).ComCodigo(5).Build();
            _repository.GetAll().Returns(new List<ContaRateio>() { contaRaizMaiorCodigo, outraContaRaiz }.AsEnumerable());

            var retorno = _service.ProximoCodigo(outraContaRaiz.ContaPaiId);
            Assert.Equal(contaRaizMaiorCodigo.Codigo + 1, retorno.Codigo);
            Assert.Equal(Guid.Empty, retorno.IdContaPai);
            Assert.False(retorno.ContaPaiAlterada);

            _service.ProximoCodigo(contaRaizMaiorCodigo.ContaPaiId);
            Assert.Equal(contaRaizMaiorCodigo.Codigo + 1, retorno.Codigo);
            Assert.Equal(Guid.Empty, retorno.IdContaPai);
            Assert.False(retorno.ContaPaiAlterada);
        }

        [Fact]
        public void Validar_Codigo_Maximo_Conta_Raiz()
        {
            //(sugestao pra aq)
            //Maior do mesmo nível 999
            //Esperado: Alerta maior código utilizado
            var contaRaiz = new ContaRateioBuilder().ComId(Guid.NewGuid()).ComCodigo(999).Build();
            _repository.GetAll().Returns(new List<ContaRateio>() { contaRaiz }.AsEnumerable());

            var ex = Assert.Throws<NegocioFinanceiroException>(() => _service.ProximoCodigo(contaRaiz.ContaPaiId));
            Assert.Equal(MensagensDeUsuario.MAIOR_CODIGO_DE_CONTA_UTILIZADO, ex.Message);
        }
        #endregion Proximo Codigo
    }
}
