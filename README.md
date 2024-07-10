# Learning_OpenTK
Learning the basic of OpenTK

A pasta Create Windows tem um programa basico que executa uma tela em branco

lembrete: as anatoções são meu entendimento e podem não estar necessariamente corretas e estão sujetas a mudar.

OpenTK tem suas proprias abstrações para lidar com as configurações de tela, como GameWindowsSettings(configurações da tela) e
NativeWindowSettings(a configuração nativa do usuario).

OnUpdateFrame da um update a cada frame e o OnRenderFrame a cada render.

OnLoad executa uma vez quando o programa esta começando, OnUnload executa quando o programa esta fechando e
OnResize é chamando quando o tamanho é alterado(viewport é a tela inteira).
