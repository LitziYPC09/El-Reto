using Emgu.CV;
using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Net.Http;
using System.IO;
namespace DeteccionPersonasWindowsForms
{
    partial class Form1
    {
        private VideoCaptureDevice videoSource;  // Declaración global de la cámara
        private System.Windows.Forms.Button btnCapturar;
        private Bitmap ultimaImagen;
        private System.Windows.Forms.Label lblResultado;
        private System.Windows.Forms.PictureBox pictureBox1;
        private VideoCapture _capture;
        private string _imagePath = "captura.jpg";
        private string _apiKey = "TU_API_KEY";
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        /// 

        private void Form1_Load(object sender, EventArgs e)
        {
            InicializarCamara();
        }

        private void InitializeComponent()
        {
            // Inicializamos los controles
            pictureBox1 = new System.Windows.Forms.PictureBox();
            btnCapturar = new System.Windows.Forms.Button();
            Button btnAnalizar = new System.Windows.Forms.Button();
            lblResultado = new System.Windows.Forms.Label();

            ((System.ComponentModel.ISupportInitialize)(pictureBox1)).BeginInit();
            this.SuspendLayout();

            // pictureBox1
            pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pictureBox1.Location = new System.Drawing.Point(20, 20);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(320, 240);
            pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;

            // btnCapturar
            btnCapturar.Location = new System.Drawing.Point(20, 280);
            btnCapturar.Name = "btnCapturar";
            btnCapturar.Size = new System.Drawing.Size(150, 40);
            btnCapturar.TabIndex = 1;
            btnCapturar.Text = "Capturar Imagen";
            btnCapturar.UseVisualStyleBackColor = true;
            btnCapturar.Click += new System.EventHandler(this.btnCapturar_Click);

            // btnAnalizar
            btnAnalizar.Location = new System.Drawing.Point(190, 280);
            btnAnalizar.Name = "btnAnalizar";
            btnAnalizar.Size = new System.Drawing.Size(150, 40);
            btnAnalizar.TabIndex = 2;
            btnAnalizar.Text = "Analizar Imagen";
            btnAnalizar.UseVisualStyleBackColor = true;
            btnAnalizar.Click += new System.EventHandler(this.btnAnalizar_Click);


            // lblResultado
            lblResultado.AutoSize = true;
            lblResultado.Location = new System.Drawing.Point(20, 340);
            lblResultado.Name = "lblResultado";
            lblResultado.Size = new System.Drawing.Size(63, 15);
            lblResultado.TabIndex = 3;
            lblResultado.Text = "Resultado:";

            // Form1
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(360, 380);
            this.Controls.Add(lblResultado);
            this.Controls.Add(btnAnalizar);
            this.Controls.Add(btnCapturar);
            this.Controls.Add(pictureBox1);
            this.Name = "Form1";
            this.Text = "Detección de Personas";
            ((System.ComponentModel.ISupportInitialize)(pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

            // Suscribimos el evento Load para inicializar la cámara
            this.Load += new System.EventHandler(this.Form1_Load);
        }


        private void InicializarCamara()
        {
            var dispositivos = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            if (dispositivos.Count == 0)
            {
                MessageBox.Show(" No se encontraron cámaras disponibles.");
                btnCapturar.Enabled = false;
                return;
            }

            // Selecciona la primera cámara de la lista
            videoSource = new VideoCaptureDevice(dispositivos[0].MonikerString);
            videoSource.NewFrame += new NewFrameEventHandler(CapturarFrame);
            videoSource.Start();
        }

        private void CapturarFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            try
            {
                // Validación adicional para asegurarnos de que pictureBox1 esté inicializado y no sea null.
                if (pictureBox1 == null || pictureBox1.IsDisposed)
                {
                    MessageBox.Show(" pictureBox1 no está disponible o fue descartado.");
                    return; //  Salimos del método para evitar errores
                }

                // Liberar memoria de la imagen anterior, si existe
                if (ultimaImagen != null)
                {
                    ultimaImagen.Dispose(); // Evitar la fuga de memoria al liberar la imagen anterior
                }

                // Clonar el fotograma capturado
                ultimaImagen = (Bitmap)eventArgs.Frame.Clone();

                // Ejecutar en el hilo de UI
                pictureBox1.Invoke(new Action(() =>
                {
                    if (!pictureBox1.IsDisposed)
                    {
                        pictureBox1.Image = (Bitmap)ultimaImagen.Clone(); // Actualizar la imagen en el PictureBox
                    }
                    else
                    {
                        MessageBox.Show(" pictureBox1 ya no está disponible (está disposed).");
                    }
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show($" Error al capturar el fotograma: {ex.Message}");
            }
        }


        private void btnCapturar_Click(object sender, EventArgs e)
        {
            if (ultimaImagen != null)
            {
                try
                {
                    // Guardar la imagen en el disco
                    ultimaImagen.Save("captura_afrorge.jpg");
                    if (lblResultado != null)
                    {
                        lblResultado.Text = " Imagen capturada correctamente: captura_afrorge.jpg";
                    }
                    else
                    {
                        MessageBox.Show(" lblResultado no está inicializado.");
                    }

                    if (lblResultado != null)
                    {
                        lblResultado.Text = " Imagen capturada correctamente: captura_afrorge.jpg";
                    }
                    else
                    {
                        MessageBox.Show(" lblResultado no está inicializado.");
                    }
                    //lblResultado.Text = "Imagen capturada correctamente: captura_afrorge.jpg";
                    //pictureBox1.Image = Image.FromFile("captura_afrorge.jpg");
                    if (lblResultado != null)
                    {
                        lblResultado.Text = " Imagen capturada correctamente: captura_afrorge.jpg";
                    }
                    else
                    {
                        MessageBox.Show(" lblResultado no está inicializado.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($" Error al guardar la imagen: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show(" No se pudo capturar la imagen.");
            }
        }


        private async void btnAnalizar_Click(object sender, EventArgs e)
        {
            string imagePath = "captura_afrorge.jpg";

            if (!File.Exists(imagePath))
            {
                MessageBox.Show(" No hay una imagen capturada para analizar.");
                return;
            }

            // Configuración de Azure
            string endpoint = "https://<tu-endpoint>.cognitiveservices.azure.com/vision/v3.2/analyze";
            string apiKey = "TU_API_KEY";

            try
            {
                byte[] imageData = File.ReadAllBytes(imagePath);
                var content = new ByteArrayContent(imageData);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apiKey);

                    // Parámetros de análisis: personas y objetos
                    string requestParameters = "visualFeatures=Objects";
                    string uri = $"{endpoint}?{requestParameters}";

                    var response = await client.PostAsync(uri, content);
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonResponse);

                    bool personaDetectada = false;

                    // Buscamos "person" en los objetos detectados
                    foreach (var obj in result.objects)
                    {
                        if (obj.objectProperty == "person")
                        {
                            personaDetectada = true;
                            break;
                        }
                    }

                    // Mostrar resultado en el label
                    lblResultado.Invoke(new Action(() =>
                    {
                        lblResultado.Text = personaDetectada ? " Persona detectada en la imagen." : "❌ No se detectó una persona en la imagen.";
                    }));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error al analizar la imagen: {ex.Message}");
            }
        }


        #endregion
    }
}

