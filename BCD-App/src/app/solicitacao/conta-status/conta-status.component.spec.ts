import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ContaStatusComponent } from './conta-status.component';

describe('ContaStatusComponent', () => {
  let component: ContaStatusComponent;
  let fixture: ComponentFixture<ContaStatusComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ContaStatusComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ContaStatusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
