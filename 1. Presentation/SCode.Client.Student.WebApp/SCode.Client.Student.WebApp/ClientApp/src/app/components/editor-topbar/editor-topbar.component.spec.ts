import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditorTopbarComponent } from './editor-topbar.component';

describe('EditorTopbarComponent', () => {
  let component: EditorTopbarComponent;
  let fixture: ComponentFixture<EditorTopbarComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditorTopbarComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditorTopbarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
