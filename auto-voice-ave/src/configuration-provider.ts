import * as vscode from 'vscode';

export class ConfigurationProvider {

    public showNotifications(): boolean {
        const settingsKey = "auto-voice-ave.showNotifications";
        let val = this.getConfiguration().get<boolean>(settingsKey);
        if (val) { return val; }
        else {
            console.error(settingsKey + " not found");
            return true;
        }
    }

    public defaultListen(): boolean {
        const settingsKey = "auto-voice-ave.defaultListen";
        let val = this.getConfiguration().get<boolean>(settingsKey);

        if (val) { return val; }
        else {
            console.error(settingsKey + " not found");
            return true;
        }
    }

    private getConfiguration() {
        return vscode.workspace.getConfiguration('auto-voice-ave');
    }
}