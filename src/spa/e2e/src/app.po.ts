import { browser, by, element } from 'protractor';

export class AppPage {
  navigateTo(): Promise<string> {
    return browser.get(browser.baseUrl) as Promise<any>;
  }

  getTitleText(): Promise<string> {
    return element(by.css('app-root .content span')).getText() as Promise<string>;
  }
}
