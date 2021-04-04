import { BrowserModule } from '@angular/platform-browser';
import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './pages/shared/nav-menu/nav-menu.component';
import { HomeComponent } from './pages/home/home.component';
import { MonacoEditorModule } from 'ngx-monaco-editor';
import { FileTreeComponent } from './components/file-tree/file-tree.component';
import { TreeModule } from '@circlon/angular-tree-component';
import { FileNavItemComponent } from './components/file-nav-item/file-nav-item.component';
import { LoggerModule} from 'ngx-logger';
import { StatusIndicatorComponent } from './components/status-indicator/status-indicator.component';
import {EditorComponent} from "./components/editor/editor.component";
import {EditorTopbarComponent} from "./components/editor-topbar/editor-topbar.component";
import {SidebarComponent} from "./components/sidebar/sidebar.component";
import {environment} from "../environments/environment";


@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    FileTreeComponent,
    FileNavItemComponent,
    StatusIndicatorComponent,
    EditorComponent,
    EditorTopbarComponent,
    SidebarComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: ':hubRoomName', component: HomeComponent },
    ]),
    MonacoEditorModule.forRoot(), // use forRoot() in main app module only.,
    TreeModule,
    LoggerModule.forRoot({
      level: environment.ngxLoggerLevel,
      disableConsoleLogging: false
    })
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  providers: [
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
