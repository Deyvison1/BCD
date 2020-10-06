import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { ContaComponent } from "./conta/conta.component";
import { ContaSComponent } from './solicitacao/conta-s/conta-s.component';
import { ContaStatusComponent } from './solicitacao/conta-status/conta-status.component';
import { SolicitacaoComponent } from "./solicitacao/solicitacao.component";

const routes: Routes = [
  { path: 'conta', component: ContaComponent },
  { path: 'solicitacao', component: SolicitacaoComponent, children: 
  [
    { path: 'conta', component: ContaSComponent },
    { path: 'status', component: ContaStatusComponent },
    { path: '', redirectTo: 'conta', pathMatch: 'full' },
    { path: '**', redirectTo: 'conta', pathMatch: 'full' }
  ] },
  { path: '', redirectTo: 'conta', pathMatch: 'full' },
  { path: '**', redirectTo: 'conta', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
