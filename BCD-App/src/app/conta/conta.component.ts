import { Component, OnInit } from '@angular/core';
import { ContaService } from '../Services/ContaServices/conta.service';
import { Conta } from '../Models/Conta';
import { Historico } from '../Models/Historico';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HelperConta } from '../Models/HelperConta';
import { PessoaService } from '../Services/PessoaServices/pessoa.service';
import { Pessoa } from '../Models/Pessoa';


@Component({
  selector: 'app-conta',
  templateUrl: './conta.component.html',
  styleUrls: ['./conta.component.css']
})
export class ContaComponent implements OnInit {

  // VARIAVEIS TIPOS PRIMITIVOS
  valorTotalCorrente: number;
  valorTotalPoupanca: number;

  // VARIAVEIS TIPO CLASS
  formNewDeposito: FormGroup;

  // VARIAVEIS DE INSTANCIAS
  conta: Conta = new Conta();
  contas: Conta[] = [];
  historico: Historico = new Historico();
  helperConta: HelperConta = new HelperConta();
  pessoas: Pessoa[] = [];
  pessoa: Pessoa = new Pessoa();

  // VARIAVEIS DE LISTAS
  historicos: Historico[] = [];
  historicoTransferencia: Historico[] = [];
  contaTransferencia: Conta[] = [];



  constructor(
    private contaService: ContaService,
    private fb: FormBuilder,
    private pessoaService: PessoaService
  ) { }

  ngOnInit() {
    this.getAllByIdPessoa();
    this.validationHelperConta();
  }

  abrirModal(template: any) {
    template.show();
  }

  abrirModalExtrato(template: any, _historicos: Historico[]) {
    this.abrirModal(template);
  }

  abrirModalDeposito(template: any) {
    this.abrirModal(template);
  }

  abrirModalTransferencia(template: any) {
    this.abrirModal(template);

    this.historicos
}

  newDeposito(helperConta: HelperConta) {

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
              });
            }, error => {
              console.log(error);
            }
          );
        });
        console.log(pessoas);
      }, error => {
        console.log(error);
      }
    );
  }

  validationHelperConta() {
    this.formNewDeposito = this.fb.group(
      {
        conta: ['', [Validators.required, Validators.max(99999999), Validators.min(10000000)]],
        agencia: ['', [Validators.required, Validators.max(99999), Validators.min(10000)]],
        nomeConta: ['', [Validators.required, Validators.maxLength(25)]]
      }
    );

  }
}
