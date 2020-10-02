import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ContaSComponent } from './conta-s.component';

describe('ContaSComponent', () => {
  let component: ContaSComponent;
  let fixture: ComponentFixture<ContaSComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ContaSComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ContaSComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
