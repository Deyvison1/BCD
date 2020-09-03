import { Component, OnInit } from '@angular/core';
import { ContaService } from '../Services/ContaServices/conta.service';
import { Conta } from '../Models/Conta';
import { Historico } from '../Models/Historico';
import { FormBuilder, FormGroup, Validators, FormArray } from '@angular/forms';
import { HelperConta } from '../Models/HelperConta';
import { PessoaService } from '../Services/PessoaServices/pessoa.service';
import { Pessoa } from '../Models/Pessoa';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-conta',
  templateUrl: './conta.component.html',
  styleUrls: ['./conta.component.css']
})
export class ContaComponent implements OnInit {

  // VARIAVEIS TIPOS PRIMITIVOS
  valorTotalCorrente: number;
  valorTotalPoupanca: number;  
  data: Date;

  // VARIAVEIS TIPO CLASS
  formNewTransferencia: FormGroup;
  formDeposito: FormGroup;
  
  // VARIAVEIS DE INSTANCIAS
  conta: Conta = new Conta();
  contas: Conta[] = [];
  historico: Historico = new Historico();
  helperConta: HelperConta = new HelperConta();
  pessoas: Pessoa[] = [];
  pessoa: Pessoa = new Pessoa();

  // VARIAVEIS DE LISTAS
  historicos: Historico[] = [];
  contaTransferencia: Conta[] = [];


  constructor(
    private contaService: ContaService,
    private fb: FormBuilder,
    private pessoaService: PessoaService,
    private toastr: ToastrService
  ) { }

  ngOnInit() {
    this.validationValorDeposito();
    this.getAllByIdPessoa();
    this.validationHelperConta();
  }

  abrirModal(template: any) {
    template.show();
  }

  abrirModalExtrato(template: any) {
    this.abrirModal(template);
    
    this.historicos.filter(x => x.dataTransacao.toString() === Date.now.toString());
  }

  abrirModalDeposito(template: any) {
    this.abrirModal(template);
  }

  abrirModalTransferencia(template: any) {
    this.abrirModal(template);

    this.historicos = this.historicos.filter(x => x.operacao == 4);
  }

  // DEPOSITO
  deposito(modal: any) {
    this.contas.forEach(
      x => {
        this.helperConta.conta = x.digitosConta;
        this.helperConta.agencia = x.digitosAgencia;
      }
    );

    this.contaService.deposito(this.helperConta).subscribe(
      (data: Conta) => {
        this.getAllByIdPessoa();
        modal.hide();
        this.helperConta.senha = '';
        this.helperConta.quantia = null;
        this.toastr.success('Sucesso no deposito!');
      }, error => {
        this.toastr.error(error);
      }  
    );
  }

  // TRANSFERENCIA
  transferencia(template: any, form: any, historico: Historico) {
    
    if(historico != null) 
    {
      this.helperConta.agenciaDestino = historico.digitosAgenciaDestino;
      this.helperConta.contaDestino = historico.digitosContaDestino;
      this.helperConta.agencia = historico.digitosAgencia;
      this.helperConta.conta = historico.digitosConta;
    }
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
                this.valorTotalCorrente =  (x.tipoConta == 0)? x.saldo : 0 ;
                this.valorTotalPoupanca =  (x.tipoConta == 1)? x.saldo : 0 ;
              });
              console.log(this.contas);
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

  validationHelperConta() {
    this.formNewTransferencia = this.fb.group(
      {
        conta: ['', [Validators.required, Validators.max(99999999), Validators.min(10000000)]],
        agencia: ['', [Validators.required, Validators.max(99999), Validators.min(10000)]],
        nomeConta: ['', [Validators.required, Validators.maxLength(25)]],
        valor: ['',]
      }
    );
  }
  validationValorDeposito() {
    this.formDeposito = this.fb.group({
      valorDeposito: ['', Validators.required],
      senha: ['', Validators.required]
    });
  }
}
