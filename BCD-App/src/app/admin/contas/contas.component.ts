import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Conta } from 'src/app/Models/Conta';
import { Endereco } from 'src/app/Models/Endereco';
import { ContaService } from 'src/app/Services/ContaServices/conta.service';

@Component({
  selector: 'app-contas',
  templateUrl: './contas.component.html',
  styleUrls: ['./contas.component.css']
})
export class ContasComponent implements OnInit {

  // PRIMITIVOS
  pageAtual = 1;
  // FORMS
  form: FormGroup;

  // ENTIDADEA
  conta: Conta = new Conta();
  endereco: Endereco = new Endereco();
  // LIST
  contas: Conta[] = [];

  constructor(
    private contaService: ContaService
  ) { }

  ngOnInit() {
    this.getAll();
  }
  abrirModal(template: any) {
    template.show();
    this.form.reset();
  }

  detalhes() {

  }

  excluir() {
    
  }

  salvarAlteracao() {

  }

  getAll() {
    return this.contaService.getAll().subscribe(
      (contas: Conta[]) =>{
        this.contas = contas;
      }, err => { console.log(err.error); }
    ); 
  }

}
