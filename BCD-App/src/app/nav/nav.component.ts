import { Component, OnInit } from '@angular/core';
import { Conta } from '../Models/Conta';
import { ContaService } from '../Services/ContaServices/conta.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  nome: string;
  logado = false;

  constructor(
    private contaService: ContaService
  ) { }

  ngOnInit() {
    this.nomeLogado();
  }

  nomeLogado() {
    this.contaService.getById(1).subscribe(
      (conta: Conta) => {
        this.nome = conta.nomeConta;
      }, error => {
        console.log(error);
      }
    );
  }

}
