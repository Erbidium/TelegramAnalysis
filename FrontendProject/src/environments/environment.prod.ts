export const environment = {
  production: true,
  //@ts-ignore
  coreUrl: window["env"].coreUrl || 'https://localhost:44357',
  //@ts-ignore
  analysisUrl: window["env"].analysisUrl || 'http://127.0.0.1:5000/'
};
