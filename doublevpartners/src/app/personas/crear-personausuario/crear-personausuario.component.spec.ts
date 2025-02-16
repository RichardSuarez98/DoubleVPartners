import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrearPersonausuarioComponent } from './crear-personausuario.component';

describe('CrearPersonausuarioComponent', () => {
  let component: CrearPersonausuarioComponent;
  let fixture: ComponentFixture<CrearPersonausuarioComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CrearPersonausuarioComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CrearPersonausuarioComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
