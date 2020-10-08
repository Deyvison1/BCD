import { Component, OnInit } from "@angular/core";
import { FormArray, FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Router } from '@angular/router';
import { ToastrService } from "ngx-toastr";
import { Conta } from "src/app/Models/Conta";
import { Endereco } from "src/app/Models/Endereco";
import { Pessoa } from "src/app/Models/Pessoa";
import { SolicitarConta } from "src/app/Models/SolicitarConta";
import { ContaService } from "src/app/Services/ContaServices/conta.service";
import { EnderecoService } from "src/app/Services/EnderecoServices/endereco.service";

@Component({
  selector: "app-conta-s",
  templateUrl: "./conta-s.component.html",
  styleUrls: ["./conta-s.component.css"],
})
export class ContaSComponent implements OnInit {
  public loading = false;
  // VARIAEVEIS TIPO PRIMITIVO
  isCollapsed = false;

  // FormGroup
  form: FormGroup;

  // VARIAVEIS DE INSTANCIAS
  solicitarConta: SolicitarConta = new SolicitarConta();
  enderecoInstancia: Endereco = new Endereco();

  // LISTAS
  contas: Conta[] = [];


  constructor(
    private fb: FormBuilder,
    private enderecoService: EnderecoService,
    private toastr: ToastrService,
    private contaService: ContaService,
    private route: Router
  ) {}

  ngOnInit() {
    this.validation();
  }

  solicitar() {
    this.solicitarConta = Object.assign({}, this.form.value);

    this.enderecoService.getEnderecoByCep(this.solicitarConta.cep).subscribe(
      (endereco: Endereco) => {
        this.solicitarConta.endereco = endereco;
        console.log(this.solicitarConta);
        this.contaService.addSolicitacao(this.solicitarConta).subscribe(
          (data) => {
            this.toastr.success('Sucesso na Solicitação!');
            this.form.reset();
            this.route.navigate(['solicitacao/status']);
            console.log(data);
          }, err => { console.log(err.error); }
        );
      }, error => { console.log(error.error); }
    );
  }

  validation() {
    this.form = this.fb.group({
      tipoConta: [0],
      senha: [''],
      nomeConta: [''],
      cpf: [''],
      cep: ['']
    });
  }
}
