Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Reflection.Emit

Module Module1
    Dim client As TcpClient
    Dim Rx As StreamReader
    Dim Tx As StreamWriter
    Dim rawdata As String
    Dim dataseparate As Array
    Dim IpAdress As String
    Sub Main()

        Dim rta As String = "-"

        Console.WriteLine("Ingrese la dirección IP")
        IpAdress = Console.ReadLine()

        '// Connect to ESP'
        Try
            client = New TcpClient(IpAdress, "80")
            If client.GetStream.CanRead = True Or client.GetStream.CanWrite Then
                Rx = New StreamReader(client.GetStream)
                Tx = New StreamWriter(client.GetStream)
                Threading.ThreadPool.QueueUserWorkItem(AddressOf Connected)
            End If

        Catch ex As Exception
            Console.Write("Error al conectarse.")
            Console.WriteLine(ex)
        End Try

        '------------ Asignar Pin de forma dinamica ---------------'

        'Dim pin As String
        'Console.WriteLine("Asigne el pin a utilizar.")
        'pin = Console.ReadLine()
        'SendToServer("Pin:" + pin)

        While Not rta = "FIN"

            If rawdata Then
                Console.WriteLine("El valor de led1 es: " + rawdata)
            End If
            Console.WriteLine("Indique el estado del led(HIGH/LOW) en caso de finalizar, escriba fin")
            rta = Console.ReadLine()
            rta = rta.ToLower()
            SendToServer("led1:" + rta)

        End While


    End Sub

    Function Connected()
        If Rx.BaseStream.CanRead = True Then
            Try
                While Rx.BaseStream.CanRead = True

                    rawdata = Rx.ReadLine
                    Console.WriteLine(rawdata)

                End While
            Catch ex As Exception
                client.Close()
                Console.WriteLine("Error al recibir la request desde el servidor")
            End Try
            Rx.DiscardBufferedData()
        End If

        Return True
    End Function
    Function SendToServer(ByVal data As String)
        Try
            Tx.WriteLine(data)
            Tx.Flush()
        Catch ex As Exception
            Console.WriteLine("Error al enviar data al servidor.")
        End Try
        Return True
    End Function

End Module
