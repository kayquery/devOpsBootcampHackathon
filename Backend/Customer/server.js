const express = require('express');
const app = express();
const port = 3000; // Porta que será ouvida
const nodemailer = require('nodemailer');
require('dotenv').config();

app.use(express.urlencoded({ extended: true}));
app.use(express.json());

// Configuração do transportador de e-mail
const transportador = nodemailer.createTransport({
    service: 'gmail',
    auth: {
        user: process.env.EMAIL_USUARIO,
        pass: process.env.EMAIL_SENHA_APP,
    },
});

// Função para enviar o e-mail do suporte
const enviarEmail = async (nome, email, mensagem, emailDestino) => {
    try{
        const info = await transportador.sendMail({
            from: process.env.EMAIL_USUARIO,
            to: emailDestino,
            subject: 'Novo ticket de suporte',
            html: 
            ` <html>
                <head>
                    <style>
                    body {
                        font-family: Arial, sans-serif;
                        background-color: #F4F4F4;
                    }
                    .container {
                        max-width: 600px;
                        margin: 0 auto;
                        padding: 20px;
                        box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
                        background-color: #FFFFFF;
                        border-radius: 5px;
                    }
                    .cabecalho{
                        padding: 10px;
                        border-radius: 5px;
                        background-color: #F2F2F2;
                        text-align: center;
                    }
                    .conteudo{
                        padding: 20px;
                    }
                    </style>
                </head>
                <body>
                    <div class="container">
                        <div class="cabecalho">
                            <h1>Ticket de suporte</h1>
                        </div>
                        <div class="conteudo">
                            <p>Um novo ticket foi aberto.</p>
                            <ul>
                                <li>Solicitante: ${nome}</li>
                                <li>E-mail: ${email}</li>
                                <li>Mensagem: ${mensagem}</li>
                            </ul>
                        </div>
                    </div>
                </body>
            </html>
            `,
        });

        console.log('E-mail enviado:', info.messageId);
    } catch (error){
        console.error('Erro ao enviar o e-mail:', error);
    }
};

// Função para enviar o e-mail de confirmação do pagamento
const enviarEmailConfPagamento = async (nome, email, valor) => {
    try{
        const info = await transportador.sendMail({
            from: process.env.EMAIL_USUARIO,
            to: email,
            subject: 'Confirmação de pagamento',
            html: 
            ` <html>
                <head>
                    <style>
                    body {
                        font-family: Arial, sans-serif;
                        background-color: #F4F4F4;
                    }
                    .container {
                        max-width: 600px;
                        margin: 0 auto;
                        padding: 20px;
                        box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
                        background-color: #FFFFFF;
                        border-radius: 5px;
                    }
                    .cabecalho{
                        padding: 10px;
                        border-radius: 5px;
                        background-color: #F2F2F2;
                        text-align: center;
                    }
                    .conteudo{
                        padding: 20px;
                    }
                    </style>
                </head>
                <body>
                    <div class="container">
                        <div class="cabecalho">
                            <h1>Confirmação de Pagamento</h1>
                        </div>
                        <div class="conteudo">
                            <p>Olá ${nome},</p>
                            <p>Passando aqui para avisar que o pagamento no valor de R$${valor.toFixed(2)} foi efetivado!</p>
                            <p>Agradecemos por nos escolher!</p>
                        </div>
                    </div>
                </body>
            </html>
            `,
        });
        console.log('E-mail de confirmação de pagamento enviado:', info.messageId);
    } catch (error){
        console.error('Erro ao enviar o e-mail de confirmação de pagamento:', error);
    }
};

// Rota para receber o POST do formulário
app.post('/enviar-email', (req, res) => {
    const { nome, email, mensagem } = req.body;

    // Lógica para enviar o e-mail usando o Nodemailer
    enviarEmail(nome, email, mensagem, process.env.EMAIL_DESTINO);

    res.send('E-mail enviado com sucesso!');
});

app.post('/enviar-email-confirmacao', (req, res) => {
    const { nome, email, valor } = req.body;

    enviarEmailConfPagamento(nome, email, valor);

    res.send('E-mail de confirmação de pagamento enviado com sucesso!');
});

// Inicia o servidor
app.listen(port, () => {
    console.log(`Servidor rodando em http://localhost:${port}`);
});