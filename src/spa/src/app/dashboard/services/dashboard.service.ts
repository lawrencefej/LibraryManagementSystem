import { HttpClient } from '@angular/common/http';
import { Injectable, OnDestroy } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { tap } from 'rxjs/operators';
import { AuthService } from 'src/app/_services/authentication.service';
import { DashboardResponse } from 'src/dto/models';
import { environment } from 'src/environments/environment';

export interface DashboardResponseViewModel {
  dashboardData?: DashboardResponse;
}

@Injectable()
export class DashboardService implements OnDestroy {
  private baseUrl = environment.apiUrl + 'dashboard';
  private dashboardSubject = new BehaviorSubject<DashboardResponseViewModel>({ dashboardData: undefined });
  private hubConnection!: HubConnection;
  private hubUrl = `${environment.hubUrl}chart`;
  private readonly unsubscribe = new Subject<void>();

  constructor(private readonly httpService: HttpClient, private readonly authService: AuthService) {}

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  getDashboardObservable(): Observable<DashboardResponseViewModel> {
    return this.dashboardSubject.asObservable();
  }

  getDashboardData(): Observable<DashboardResponse> {
    return this.httpService
      .get<DashboardResponse>(`${this.baseUrl}`)
      .pipe(tap(dashboardResponse => this.dashboardSubject.next({ dashboardData: dashboardResponse })));
  }

  startConnection(): void {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(`${this.hubUrl}`, {
        accessTokenFactory: () => this.authService.userValue.token
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start().catch(err => console.log('Error while starting connection: ' + err));

    this.getData();
  }

  stopConnection(): void {
    if (this.hubConnection) {
      this.hubConnection.stop().catch(err => console.log(err));
    }
  }

  getData(): void {
    this.hubConnection.on('BroadcastChartData', (data: DashboardResponse) => {
      this.dashboardSubject.next({ dashboardData: data });
    });
  }
}
