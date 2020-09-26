import { Component, OnInit } from '@angular/core';
import { ContaService } from '../Services/ContaServices/conta.service';
import { Conta } from '../Models/Conta';
import { Historico } from '../Models/Historico';
import { FormBuilder, FormGroup, Validators, FormArray } from '@angular/forms';
import { HelperConta } from '../Models/HelperConta';
import { PessoaService } from '../Services/PessoaServices/pessoa.service';
import { Pessoa } from '../Models/Pessoa';
import { ToastrService } from 'ngx-toastr';
import { HistoricoService } from '../Services/HistoricoServices/historico.service';


@Component({
  selector: 'app-conta',
  templateUrl: './conta.component.html',
  styleUrls: ['./conta.component.css']
})
export class ContaComponent implements OnInit {

  // VARIAVEIS TIPOS PRIMITIVOS
  valorTotalCorrente: number;
  valorTotalPoupanca: number;
  pageAtualTransferencia = 1;
  pageAtualExtrato = 1;
  mes: number;
  qtdPages: number;
  mostrarValorPoupanca: boolean = false;

  // VARIAVEIS TIPO CLASS
  formNewTransferencia: FormGroup;
  formDeposito: FormGroup;
  formExtrato: FormGroup;

  // VARIAVEIS DE INSTANCIAS
  conta: Conta = new Conta();
  contas: Conta[] = [];
  todasContas: Conta[] = [];
  historico: Historico = new Historico();
  helperConta: HelperConta = new HelperConta();
  pessoas: Pessoa[] = [];
  pessoa: Pessoa = new Pessoa();

  // VARIAVEIS DE LISTAS
  historicos: Historico[] = [];
  contaTransferencia: Conta[] = [];
  historicoByMes: Historico[] = [];

  constructor(
    private contaService: ContaService,
    private fb: FormBuilder,
    private pessoaService: PessoaService,
    private toastr: ToastrService,
    private historicoService: HistoricoService
  ) { }

  ngOnInit() {
    this.validationDeposito();
    this.getAllByIdPessoa();
    this.validationFormNewTransferencia();
    this.validationFormExtrato();
  }


  alternarValorVisivelPoupanca() {
    this.mostrarValorPoupanca = !this.mostrarValorPoupanca;
  }

  enviarMes() {
    this.historicoService.getByMes(this.mes).subscribe(
      (historicos: Historico[]) => {
        this.historicoByMes = historicos;
      }, error => {
        console.log(error);
      }
    );
  }
  abrirModal(template: any) {
    template.show();
  }

  abrirModalResgatarValor(template: any) {
    this.abrirModal(template);

  }

  abrirModalExtrato(template: any) {

    this.contaService.mesAtual().subscribe(
      (mes: number) => {
        this.mes = mes;
        this.historicoService.getByMes(mes).subscribe(
          (historicos: Historico[]) => {
            this.historicoByMes = historicos;
          }, error => {
            console.log(error.error);
          }
        );
      }, error => {
        console.log(error.error);
      }
    );
    this.abrirModal(template);
  }

  abrirModalDeposito(template: any) {
    this.abrirModal(template);
  }

  abrirModalDetalhes(template: any) {
    
    this.historicos.filter(x => x.operacao === 3);
    this.abrirModal(template);
  }

  abrirModalAplicar(template: any) {
    this.abrirModal(template);

    this.contas.forEach(
      x => {
        this.helperConta.conta = x.digitosConta;
        this.helperConta.agencia = x.digitosAgencia;
        this.helperConta.tipoConta = x.tipoConta;
      }
    );
    this.formDeposito.reset();
  }


  abrirModalTransferencia(template: any) {
    this.abrirModal(template);

    this.historicos = this.historicos.filter(x => x.operacao === 4);
    this.getAllContas();
  }

  // RESGATAR VALOR
  resgatarValor(template: any) {
    this.helperConta = this.formDeposito.value;


    this.contas.forEach(x => {
      this.helperConta.agencia = x.digitosAgencia;
      this.helperConta.conta = x.digitosConta;
    });

    this.contaService.resgatarValor(this.helperConta).subscribe(
      (conta: Conta) => {
        template.hide();
        this.formDeposito.reset();
        this.getAllByIdPessoa();
        this.toastr.success('Sucesso ao Resgatar Valor!');
      }, error => {
        this.formDeposito.reset();
        this.toastr.error(error.error);
      }
    );
  }

  // APLICACAO POUPANCA
  aplicarPoupanca(template: any) {
    this.helperConta =  this.formDeposito.value;

    this.contas.forEach(x => {
      this.helperConta.agencia = x.digitosAgencia;
      this.helperConta.conta = x.digitosConta;
    });

    this.contaService.aplicarPoupanca(this.helperConta).subscribe(
      (conta: Conta) => {
        this.getAllByIdPessoa();
        template.hide();

        this.formDeposito.reset();
        this.toastr.success('Sucesso ao Aplicar Poupanca');
      }, error => {
        this.formDeposito.reset();
        this.toastr.error('Erro ao Aplicar Poupanca!');
      }
    );
  }

  // DEPOSITO
  deposito(modal: any) {

    this.helperConta = this.formDeposito.value;

    this.contas.forEach(x => {
      this.helperConta.conta = x.digitosConta;
      this.helperConta.agencia = x.digitosAgencia;
    });

    this.contaService.deposito(this.helperConta).subscribe(
      (data: Conta) => {
        this.getAllByIdPessoa();
        modal.hide();
        
        this.formDeposito.reset();
        this.toastr.success('Sucesso no deposito!');
      }, error => {
        this.formDeposito.reset();
        this.toastr.error(error.error);
      }  
    );
  }


  newTransferencia(modalNewTransferencia: any) {
    this.helperConta = this.formNewTransferencia.value;

    this.contas.forEach(x => {
      if(x.tipoConta === 0) {      
        this.helperConta.agencia = x.digitosAgencia;
        this.helperConta.conta = x.digitosConta;
      }
    });

    this.contaService.transferencia(this.helperConta).subscribe(
      (conta: Conta) => {
        this.getAllByIdPessoa();
        this.toastr.success('Sucesso na Transferencia');
        modalNewTransferencia.hide();

        this.formNewTransferencia.reset();
      }, error => {
        modalNewTransferencia.hide();

        this.formNewTransferencia.reset();
        this.toastr.error(error.error);
      }
    );
  }
  // TRANSFERENCIA
  transferencia(template: any, historico: Historico) {
  
      this.helperConta = this.formDeposito.value;
      this.helperConta.agenciaDestino = historico.digitosAgenciaDestino;
      this.helperConta.contaDestino = historico.digitosContaDestino;
      this.helperConta.agencia = historico.digitosAgencia;
      this.helperConta.conta = historico.digitosConta;
      
      this.contaService.getByContaAndAgencia(this.helperConta.contaDestino, this.helperConta.agenciaDestino).subscribe(
        (conta: Conta) => {
          this.helperConta.cpf = (this.helperConta.cpf === null)? conta.cpf : this.helperConta.cpf;

          this.helperConta.cpf = conta.cpf;
          this.contaService.transferencia(this.helperConta).subscribe(
            (conta: Conta[]) => {
              template.hide();

              this.getAllContas();
              this.getAllByIdPessoa();
              this.toastr.success('Sucesso!');
            }, error => {
              this.formDeposito.reset();
              this.toastr.error(error.error);
            }
          );
        }, error => {
          template.hide();
          this.toastr.error(error.error);
        }
      );
  }

  getAllContas() {
    this.contaService.getAllDeleteNomeCurrency('Deyvison').subscribe(
      (contas: Conta[]) => {
        this.todasContas = contas;
      }, error => {
        this.toastr.error(error.error);
      }
    );
  }
  
  getAllByIdPessoa() {
    return this.pessoaService.getAllByIdPessoa(1).subscribe(
      (pessoas: Pessoa[]) => {
        this.pessoas = pessoas;

        pessoas.forEach(x => {
          this.contas = x.contas
          
          this.contaService.getByIdList(1).subscribe(
            (conta: Conta[]) => {
              conta.forEach(x => {
                this.historicos = x.extrato;
                this.valorTotalCorrente =  (x.tipoConta === 0)? x.saldo : 0 ;
              });
              this.contas.forEach(x => {
                this.valorTotalPoupanca = (x.tipoConta === 1)? x.saldo : 0;
              });
            }, error => {
              console.log(error);
            }
          );
        });
      }, error => {
        console.log(error);
      }
    );
  }

  validationFormNewTransferencia() {
    this.formNewTransferencia = this.fb.group(
      {
        contaDestino: ['', [Validators.required, Validators.max(99999999), Validators.min(10000000) ] ],
        agenciaDestino: ['', [Validators.required, Validators.max(99999), Validators.min(10000)] ],
        cpf: ['', Validators.required ],
        nomeConta: [''],
        senha: ['', Validators.required ],
        quantia: ['', [Validators.required, Validators.min(0.1)]]
      }
    );
  }

  validationFormExtrato() {
    this.formExtrato = this.fb.group({
      mes: ['']
    });
  }

  validationDeposito() {
    this.formDeposito = this.fb.group({
      quantia: ['', [Validators.required, Validators.min(0.1)] ],
      senha: ['', Validators.required ]
    });
  }
}
