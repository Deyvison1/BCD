import { Component, OnInit } from "@angular/core";
import { FormArray, FormBuilder, FormGroup, Validators } from "@angular/forms";
import { ToastrService } from 'ngx-toastr';
import { Conta } from 'src/app/Models/Conta';
import { Endereco } from "src/app/Models/Endereco";
import { Pessoa } from 'src/app/Models/Pessoa';
import { SolicitarConta } from 'src/app/Models/SolicitarConta';
import { ContaService } from 'src/app/Services/ContaServices/conta.service';
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
  cep: number;
  
  // FormGroup
  form: FormGroup;

  // VARIAVEIS DE INSTANCIAS
  solicitarConta: SolicitarConta = new SolicitarConta();
  enderecoInstancia: Endereco = new Endereco();

  // LISTAS
  enderecosCompletos: Endereco[] = [];
  contas: Conta[] = [];


  // FormArray
  get enderecos(): FormArray {
    return <FormArray>this.form.get('enderecos');
  }

  constructor(
    private fb: FormBuilder,
    private enderecoService: EnderecoService,
    private toastr: ToastrService,
    private contaService: ContaService
    ) {}

  ngOnInit() {
    this.validation();
  }

  solicitar() {
    this.solicitarConta = Object.assign({}, this.form.value);

    this.solicitarConta.enderecos.forEach(
      x => 
      {
        this.enderecoService.getEnderecoByCep(x.cep).subscribe(
          (data: Endereco) => {
            this.solicitarConta.enderecos.push(data);

            this.solicitarConta.enderecos.splice(0, 1);
          }, error => {
            console.log(error);
          }
        );
      }
    );

    this.enderecoService.getEnderecoByCep(this.solicitarConta.cep).subscribe(
      (data: Endereco) => {
        this.solicitarConta.enderecos.push(data);
      },
      error => {
        console.log(error);
      }
    );

    console.log(this.solicitarConta);
   
  }

  buscarCep() {
  
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
