import { Component, OnInit } from "@angular/core";
import { FormArray, FormBuilder, FormGroup, Validators } from "@angular/forms";
import { ToastrService } from 'ngx-toastr';
import { Endereco } from "src/app/Models/Endereco";
import { Pessoa } from 'src/app/Models/Pessoa';
import { SolicitarConta } from 'src/app/Models/SolicitarConta';
import { EnderecoService } from 'src/app/Services/EnderecoServices/endereco.service';

@Component({
  selector: "app-conta-s",
  templateUrl: "./conta-s.component.html",
  styleUrls: ["./conta-s.component.css"],
})
export class ContaSComponent implements OnInit {
  
  public loading = false;
  // VARIAEVEIS TIPO PRIMITIVO
  isCollapsed = true;
  
  // FormGroup
  form: FormGroup;

  // VARIAVEIS DE INSTANCIAS
  solicitarConta: SolicitarConta = new SolicitarConta();
  enderecoInstancia: Endereco = new Endereco();

  // LISTAS
  enderecosCompletos: Endereco[] = [];

  objeto: any;

  // FormArray
  get enderecos(): FormArray {
    return <FormArray>this.form.get('enderecos');
  }

  constructor(
    private fb: FormBuilder,
    private enderecoService: EnderecoService,
    private toastr: ToastrService
    ) {}

  ngOnInit() {
    this.validation();
  }

  solicitar() {
    this.solicitarConta = this.form.value;


      this.solicitarConta.enderecos.forEach(x => {
        this.enderecoService.getEnderecoByCep(x.cep).subscribe(
          (endereco: Endereco) => {
            this.enderecosCompletos.push(endereco);
          }, error => {
            this.toastr.error(`Cep Invalido: ${x.cep}`);
          }
        );
      });
    this.enderecoService.getEnderecoByCep(this.solicitarConta.cep).subscribe(
      (endereco: Endereco) => {
        this.enderecosCompletos.push(endereco);
      }, error => {
        this.toastr.error(`Cep Invalido: ${this.solicitarConta.cep}`);
      }
    );
    this.solicitarConta.enderecos = this.enderecosCompletos;
    console.log(this.solicitarConta);
  }

  buscarCep() {
    this.solicitarConta = this.form.value;
    console.log(this.solicitarConta.cep);
  }

  addEndereco() {
    this.enderecos.push(this.criarEndereco({ id: 0 }));
  }

  removerEndereco(id: number) {
    this.enderecos.removeAt(id);
  }

  criarEndereco(endereco: any) {
    return this.fb.group({
      id: [endereco.id],
      cep: [endereco.cep, [Validators.required]],
      logradouro: [endereco.logradouro, [Validators.required]],
      bairro: [endereco.bairro, [Validators.required]],
      localidade: [endereco.localidade, [Validators.required]],
      uf: [endereco.uf, [Validators.required]],
    });
  }

  validation() {
    this.form = this.fb.group({
      cep: [],
      tipoConta: [0,],
      senha: [],
      nomeConta: [],
      cpf: [],
      enderecos: this.fb.array([])
    });
  }

}
