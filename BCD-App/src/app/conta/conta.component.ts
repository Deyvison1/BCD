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
  public loading = false;
  valorTotalCorrente: number;
  valorTotalPoupanca: number;
  pageAtualTransferencia = 1;
  pageAtualExtrato = 1;
  pageAtualExtratoPoupanca = 1;
  pageAtualExtratoLastMeses = 1;
  pageAtualExtratoLast = 1;
  mes: number;
  qtdPages: number;
  mostrarValorPoupanca: boolean = false;
  mensagemTooltipResgatarValor: string;
  mensagemTooltipAplicarValor: string;
  mensagemValorVisivel: string;

  // VARIAVEIS TIPO CLASS
  formNewTransferencia: FormGroup;
  formDeposito: FormGroup;
  formExtrato: FormGroup;

  // VARIAVEIS DE INSTANCIAS
  conta: Conta = new Conta();
  historico: Historico = new Historico();
  helperConta: HelperConta = new HelperConta();
  pessoas: Pessoa[] = [];
  pessoa: Pessoa = new Pessoa();

  // VARIAVEIS DE LISTAS
  contas: Conta[] = [];
  historicos: Historico[] = [];
  contaTransferencia: Conta[] = [];
  historicoByMesCorrente: Historico[] = [];
  historicoByMesPoupanca: Historico[] = [];
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
    this.loading = true;
    this.contas.forEach(x => {
      if(x.tipoConta === 0) {
        this.helperConta.conta = x.digitosConta;
        this.helperConta.agencia = x.digitosAgencia;
      }
    });

    this.historicoService.getByMesCorrente(this.mes, this.helperConta.agencia, this.helperConta.conta).subscribe(
      (historicos: Historico[]) => {
        
        this.historicoByMesCorrente = historicos;

        this.historicoService.getByMesPoupanca(this.mes, this.helperConta.agencia, this.helperConta.conta).subscribe(
          (historicosPoupanca: Historico[]) => {
            
            this.historicoByMesPoupanca = historicosPoupanca;
            this.loading = false;
          }, error => {
            console.log(error.error);
            this.loading = false;
          }
        );
      }, error => {
        console.log(error);
        this.loading = false;
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

    this.contas.forEach(x => {
      if(x.tipoConta === 0) {
        this.helperConta.conta = x.digitosConta;
        this.helperConta.agencia = x.digitosAgencia;
      }
    });
    this.contaService.mesAtual().subscribe(
      (mes: number) => {
        this.mes = mes;
        this.historicoService.getByMesCorrente(mes, this.helperConta.agencia, this.helperConta.conta).subscribe(
          (historicos: Historico[]) => {
            this.historicoByMesCorrente = historicos;

            this.historicoService.getByMesPoupanca(mes, this.helperConta.agencia, this.helperConta.conta).subscribe(
              (historicosPoupanca: Historico[]) => {
                this.historicoByMesPoupanca = historicosPoupanca;

              }, error => {
                console.log(error.error);
              }
            );
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

  lastHistoricoByMes() {
    this.historicoService.getLastMeses(this.helperConta.agencia, this.helperConta.conta, this.helperConta.tipoConta).subscribe(
      (historico: Historico[]) => {
        this.historicoByMes = historico;
      }, error => {
        console.log(error.error);
      }
    );
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

    this.contaService.getAllContasCadastradas(1).subscribe(
      (contas: Conta[]) => {
        this.contaTransferencia = contas;

        console.log(this.contaTransferencia);
      }, error => {
        console.log(error.error);
      }
    );
  }

  // RESGATAR VALOR
  resgatarValor(template: any) {
    this.loading = true;
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
        this.loading = false;
      }, error => {
        this.formDeposito.reset();
        this.toastr.error(error.error);
        this.loading = false;
      }
    );
  }

  // APLICACAO POUPANCA
  aplicarPoupanca(template: any) {
    this.loading = true;

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
        this.loading = false;
      }, error => {
        this.formDeposito.reset();
        this.toastr.error(error.error);
        this.loading = false;
      }
    );
  }

  // DEPOSITO
  deposito(modal: any) {
    this.loading = true;
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
        this.toastr.success('Sucesso no Deposito!');
        this.loading = false;
      }, error => {
        this.formDeposito.reset();
        this.toastr.error(error.error);
        this.loading = false;
      }  
    );
  }


  newTransferencia(modalNewTransferencia: any) {
    this.loading = true;

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
        this.loading = false;
      }, error => {
        modalNewTransferencia.hide();

        this.formNewTransferencia.reset();
        this.toastr.error(error.error);
        this.loading = false;
      }
    );
  }
  // TRANSFERENCIA
  transferencia(template: any, conta: Conta) {
      this.loading = true;
      this.helperConta = this.formDeposito.value;
      this.helperConta.agenciaDestino = conta.digitosAgencia;
      this.helperConta.contaDestino = conta.digitosConta;
      
      this.contas.forEach(x => {
        if(x.tipoConta === 0) {
          this.helperConta.conta = x.digitosConta;
          this.helperConta.agencia = x.digitosAgencia;
        }
      });

      
      this.contaService.getByContaAndAgencia(this.helperConta.contaDestino, this.helperConta.agenciaDestino).subscribe(
        (conta: Conta) => {
          this.helperConta.cpf = (this.helperConta.cpf === null)? conta.cpf : this.helperConta.cpf;

          this.helperConta.cpf = conta.cpf;
          this.contaService.transferencia(this.helperConta).subscribe(
            (conta: Conta[]) => {
              template.hide();

              this.getAllByIdPessoa();
              this.toastr.success('Sucesso na Transferencia!');
              this.formDeposito.reset();
              this.loading = false;
            }, error => {
              this.formDeposito.reset();
              this.toastr.error(error.error);
              this.loading = false;
            }
          );
        }, error => {
          template.hide();
          this.toastr.error(error.error);
          this.loading = false;
        }
      );
  }

  
  getAllByIdPessoa() {
    this.loading = true;
    return this.pessoaService.getAllByIdPessoa(1).subscribe(
      (pessoas: Pessoa[]) => {
        this.pessoas = pessoas;

        pessoas.forEach(x => {
          this.contas = x.contas

          this.mensagemTooltipResgatarValor = (x.contas.length === 1)? 'Conta Poupança Inexistente' : 'Resgatar Valor';
          this.mensagemTooltipAplicarValor = (x.contas.length === 1)? 'Conta Poupança Inexistente' : 'Aplicação Poupança';
          this.mensagemValorVisivel = (x.contas.length == 1)? 'Conta Poupança Inexistente' : '';
          
          this.contaService.getListByIdPessoa(1).subscribe(
            (conta: Conta[]) => {
              conta.forEach(x => {
                this.historicos = x.extrato;
                this.valorTotalCorrente =  (x.tipoConta === 0)? x.saldo : 0 ;
              });
              this.contas.forEach(x => {
                this.valorTotalPoupanca = (x.tipoConta === 1)? x.saldo : 0;
              });
              this.loading = false;
            }, error => {
              console.log(error);
              this.loading = false;
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
